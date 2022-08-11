using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.Contexts.Essential.Queries;

public class UserTokenQuery : IRequest<ResponseDto<TokenDtoV1>>
{
    private readonly IRequestDto requestDto;
    private readonly LoginDtoV1 loginDto;
    
    public UserTokenQuery(IRequestDto requestDto)
    {
        this.requestDto = requestDto;
    }

    public UserTokenQuery(IRequestDto<LoginDtoV1> dto)
    {
        requestDto = dto;
        loginDto = LoginDtoV1.Create(dto.Payload.UsernameOrEmail.ToLower(), dto.Payload.Password);
    }

    public class UserTokenHandler : IRequestHandler<UserTokenQuery, ResponseDto<TokenDtoV1>>
{
    private readonly IEssentialDbContext context;
    private readonly IJwtService jwtService;
    private readonly IMediator mediator;
    private readonly IConfiguration configuration;
    private readonly ICurrentUserInfo currentUserInfo;
    private readonly IPasswordSecurity passwordSecurity;

    public UserTokenHandler(
        IEssentialDbContext context, 
        IJwtService jwtService, 
        IMediator mediator, 
        IConfiguration configuration,
        ICurrentUserInfo currentUserInfo,
        IPasswordSecurity passwordSecurity)
    {
        this.context = context;
        this.jwtService = jwtService;
        this.mediator = mediator;
        this.configuration = configuration;
        this.currentUserInfo = currentUserInfo;
        this.passwordSecurity = passwordSecurity;
    }

    public async Task<ResponseDto<TokenDtoV1>> Handle(UserTokenQuery request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.loginDto?.UsernameOrEmail) && !string.IsNullOrWhiteSpace(request.loginDto?.Password))
        {
            var employee = await context.Users.FirstOrDefaultAsync(x =>
                (x.Username == request.loginDto.UsernameOrEmail || x.Email == request.loginDto.UsernameOrEmail) &&
                x.Password == passwordSecurity.Hash(request.loginDto.Password), cancellationToken);

            if (employee is null)
            {
                employee = await context.Users.FirstOrDefaultAsync(x =>
                    (x.Username == request.loginDto.UsernameOrEmail || x.Email == request.loginDto.UsernameOrEmail) &&
                    (x.TempPassword == passwordSecurity.Hash(request.loginDto.Password) && x.TempPasswordTs > DateTime.UtcNow.AddMinutes(-10))
                    , cancellationToken);
                if (employee is null)
                    throw new NotFoundException("User not found!");
            }

            if (employee.SuspendArgs is not null)
            {
                throw new NotAllowedException("Account is locked!");
            }

            if (employee.TempPassword != null)
            {
                if (request.loginDto.Password == employee.TempPassword)
                {
                    employee.UpdatePassword(employee.TempPassword);
                }

                employee.SetTemporaryPassword(null);
            }
            
            
            await context.SaveChangesAsync(cancellationToken);

            var session = await context.UserSessions.FirstOrDefaultAsync(x => x.UserId == employee.UserId && x.DeviceId == request.requestDto.DeviceId && x.TerminationTs == null, cancellationToken);
            if (session is null)
            {
                session = UserSessionDbo.Create(employee.UserId, request.requestDto.DeviceId);
                await context.UserSessions.AddAsync(session, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                await context.SessionLogs.AddAsync(session.CreateLog("Login", "Created New Session."), cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            var ci = new ClaimsIdentity("custom");
            ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Session, session.SessionId.ToString()));

            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = ci,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtService.GenerateToken(descriptor);

            return ResponseDto.Create(TokenDtoV1.Create(token, employee.FirstName + " " + employee.LastName, " url"));
        }
        
        if (currentUserInfo.SessionId is not null)
        {
            var session = await context.UserSessions.FirstOrDefaultAsync(x => x.SessionId == currentUserInfo.SessionId && x.TerminationTs == null && x.DeviceId == request.requestDto.DeviceId, cancellationToken);
            if (session is null) throw new NotAllowedException("Different Device!!! or session expired");

            //session.Logs.Add(new SessionLog("Verified."));
            await context.SaveChangesAsync(cancellationToken);

            //var token = await GetToken(session.SessionId, DateTime.UtcNow.AddDays(5), cancellationToken);
            
            var employee = await
                context.Users.Include(x => x.Claims).FirstOrDefaultAsync(x => x.UserId == session.UserId, cancellationToken);
            
            var ci = new ClaimsIdentity("custom");
            ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Session, session.SessionId.ToString()));
            if (employee?.TerritoryId is not null)
            {
                ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Territory, employee.TerritoryId));
            }
            //foreach (var c in employee.Claims.Where(x => x.RevokedTs == null).ToList())
            foreach (var c in employee.Claims?.Where(x => x.RevokedTs == null)?.ToList())
            {
                ci.AddClaim(new Claim(c.ClaimType, c.ClaimValue));
            }
            
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = ci,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtService.GenerateToken(descriptor);

            return ResponseDto.Create(TokenDtoV1.Create(token, employee.FirstName + " " + employee.LastName, "url"));
        }
        
        throw new BadRequestException("Invalid request query parameters.");
    }

    private async Task<string> GetToken(Guid sessionId, DateTime? expires, CancellationToken cancellationToken)
    {
        var query = await context.UserClaims
            .Include(u => u.User)
            .ThenInclude(s => s.Sessions)
            .Where(x => x.User.Sessions.Select(y => y.SessionId).FirstOrDefault() == sessionId)
            .Select(c => new
            {
                c.User.TerritoryId,
                c.ClaimType,
                c.ClaimValue,
                c.RevokedTs
            })
            .Where(x => x.RevokedTs == null)
            .ToListAsync(cancellationToken);
        
        var ci = new ClaimsIdentity("custom");
        ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Session, sessionId.ToString()));
        ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Territory, query?.FirstOrDefault()?.TerritoryId ?? string.Empty));

        foreach (var c in query)
        {
            ci.AddClaim(new Claim(c.ClaimType, c.ClaimValue));
        }

        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = ci,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return jwtService.GenerateToken(descriptor);
    }
}
}