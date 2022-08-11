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

public class UserTokenQuery : IRequest<ResponseDto<AuthenticationResponseDto>>
{
    private readonly IRequestDto requestDto;
    private readonly AuthenticationRequestDto loginDto;
    
    public UserTokenQuery(IRequestDto requestDto)
    {
        this.requestDto = requestDto;
    }

    public UserTokenQuery(IRequestDto<AuthenticationRequestDto> dto)
    {
        requestDto = dto;
        loginDto = dto.Payload;
    }

    public class UserTokenHandler : IRequestHandler<UserTokenQuery, ResponseDto<AuthenticationResponseDto>>
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

        public async Task<ResponseDto<AuthenticationResponseDto>> Handle(UserTokenQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.loginDto?.UsernameOrEmail) && !string.IsNullOrWhiteSpace(request.loginDto?.Password))
            {
                var user = await context.Users
                    .Include(x => x.Person)
                    .FirstOrDefaultAsync(x =>
                        (x.Username == request.loginDto.UsernameOrEmail || x.Email == request.loginDto.UsernameOrEmail) &&
                        x.Password == passwordSecurity.Hash(request.loginDto.Password), cancellationToken);

                if (user is null)
                {
                    user = await context.Users
                        .Include(x => x.Person)
                        .FirstOrDefaultAsync(x =>
                                (x.Username == request.loginDto.UsernameOrEmail || x.Email == request.loginDto.UsernameOrEmail) &&
                                (x.TempPassword == passwordSecurity.Hash(request.loginDto.Password) && x.TempPasswordTs > DateTime.UtcNow.AddMinutes(-10))
                            , cancellationToken);
                    if (user is null)
                        throw new NotFoundException("User not found!");
                }

                if (user.SuspendArgs is not null)
                {
                    throw new NotAllowedException("Account is locked!");
                }

                if (user.TempPassword != null)
                {
                    if (request.loginDto.Password == user.TempPassword)
                    {
                        user.UpdatePassword(user.TempPassword);
                    }

                    user.SetTemporaryPassword(null);
                }
            
            
                await context.SaveChangesAsync(cancellationToken);

                var session = await context.UserSessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.DeviceId == request.requestDto.DeviceId && x.TerminationTs == null, cancellationToken);
                if (session is null)
                {
                    session = UserSessionDbo.Create(user.UserId, request.requestDto.DeviceId);
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

                return ResponseDto.Create(AuthenticationResponseDto.Create(token, user.Person?.FirstName + " " + user.Person?.LastName));
            }
        
            if (currentUserInfo.SessionId is not null)
            {
                var session = await context.UserSessions.FirstOrDefaultAsync(x => x.SessionId == currentUserInfo.SessionId && x.TerminationTs == null && x.DeviceId == request.requestDto.DeviceId, cancellationToken);
                if (session is null) throw new NotAllowedException("Different Device!!! or session expired");
            
                await context.SaveChangesAsync(cancellationToken);

                var user = await
                    context.Users.Include(x => x.Claims)
                        .FirstOrDefaultAsync(x => x.UserId == session.UserId, cancellationToken);
            
                var ci = new ClaimsIdentity("custom");
                ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Session, session.SessionId.ToString()));

                var employee = await context.Employees.FirstOrDefaultAsync(x => x.UserId == user.UserId, cancellationToken);
                if (employee is not null)
                {
                    ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Territory, employee.TerritoryId.ToString()));
                }

                var customer =
                    await context.Customers.FirstOrDefaultAsync(x => x.UserId == user.UserId, cancellationToken);
                if (customer is not null)
                {
                    ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Country, customer.CountryCode));
                }
           
            
                foreach (var c in user?.Claims?.Where(x => x.RevokedTs == null)?.ToList())
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

                return ResponseDto.Create(AuthenticationResponseDto.Create(token, user.Person?.FirstName + " " + user.Person?.LastName));
            }
        
            throw new BadRequestException("Invalid request query parameters.");
        }
    }
}