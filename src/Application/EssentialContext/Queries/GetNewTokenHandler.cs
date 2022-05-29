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
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.SharedKernel.Essential.Security;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Queries;

public class GetNewTokenHandler : IRequestHandler<GetNewTokenQuery, ResponseDto<TokenDtoV1>>
{
    private readonly IEssentialDbContext context;
    private readonly IJwtService jwtService;
    private readonly IMediator mediator;
    private readonly IConfiguration configuration;
    private readonly ICurrentUserInfo currentUserInfo;
    private readonly IPasswordSecurity passwordSecurity;

    public GetNewTokenHandler(
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

    public async Task<ResponseDto<TokenDtoV1>> Handle(GetNewTokenQuery request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.Payload?.UsernameOrEmail) && !string.IsNullOrWhiteSpace(request.Payload?.Password))
        {
            var employee = await context.Users.FirstOrDefaultAsync(x =>
                (x.Username == request.Payload.UsernameOrEmail || x.Email == request.Payload.UsernameOrEmail) &&
                x.Password == passwordSecurity.Hash(request.Payload.Password), cancellationToken);

            if (employee is null)
            {
                employee = await context.Users.FirstOrDefaultAsync(x =>
                    (x.Username == request.Payload.UsernameOrEmail || x.Email == request.Payload.UsernameOrEmail) &&
                    (x.TempPassword == passwordSecurity.Hash(request.Payload.Password) && x.TempPasswordTs > DateTime.UtcNow.AddMinutes(-10))
                    , cancellationToken);
                if (employee is null)
                    throw new NotFoundException();
            }

            if (employee.SuspendArgs is not null)
            {
                throw new NotAllowedException();
            }

            if (employee.TempPassword != null)
            {
                employee.UpdatePassword(employee.TempPassword);
            }
            
            employee.SetTemporaryPassword(null);
            await context.SaveChangesAsync(cancellationToken);

            var session = await context.Sessions.FirstOrDefaultAsync(x => x.UserId == employee.UserId && x.DeviceId == request.Request.DeviceId && x.TerminationTs == null, cancellationToken);
            if (session is null)
            {
                session = SessionDbo.Create(employee.UserId, request.Request.DeviceId);
                await context.Sessions.AddAsync(session, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                await context.Logs.AddAsync(session.CreateLog("Login", "Created New Session."), cancellationToken);
            }
            // else
            // {
            //     if (session.DeviceId != request.Request.DeviceId)
            //     {
            //         session.TerminationTs = DateTime.UtcNow;
            //         await context.SaveChangesAsync(cancellationToken);
            //         throw new NotAllowedException("User used session not related to device!");
            //     }
            // }

            await context.SaveChangesAsync(cancellationToken);

            var ci = new ClaimsIdentity("custom");
            ci.AddClaim(new Claim(ClaimsTagsList.Claims.Session, session.SessionId.ToString()));

            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = ci,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtService.GenerateToken(descriptor);

            return new ResponseDto<TokenDtoV1>( TokenDtoV1.Create(token, employee.Username, "url"));
        }
        else if (currentUserInfo.GetSessionId() is not null)
        {
            var session = await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == currentUserInfo.GetSessionId() && x.TerminationTs == null && x.DeviceId == request.Request.DeviceId, cancellationToken);
            if (session is null) throw new NotAllowedException("Different Device!!! or session expired");

            //session.Logs.Add(new SessionLog("Verified."));
            await context.SaveChangesAsync(cancellationToken);

            var employee = await
                context.Users.Include(x => x.Claims).FirstOrDefaultAsync(x => x.UserId == session.UserId, cancellationToken);



            var ci = new ClaimsIdentity("custom");
            ci.AddClaim(new Claim(ClaimsTagsList.Claims.Session, session.SessionId.ToString()));
            if (employee?.TerritoryId is not null)
            {
                ci.AddClaim(new Claim(ClaimsTagsList.Claims.Territory, employee.TerritoryId));
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

            return new ResponseDto<TokenDtoV1>(TokenDtoV1.Create(token, employee.Username, "url"));
        }
        else
        {
            throw new BadRequestException("Invalid request query parameters.");
        }
    }
}