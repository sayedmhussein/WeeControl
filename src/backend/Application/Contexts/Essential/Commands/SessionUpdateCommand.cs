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
using WeeControl.ApiApp.Application.Exceptions;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.Contexts.Essential.DataTransferObjects.User;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.ApiApp.Application.Contexts.Essential.Commands;

public class SessionUpdateCommand : IRequest<ResponseDto<AuthenticationResponseDto>>
{
    private readonly IRequestDto dto;
    private readonly string otp;

    public SessionUpdateCommand(IRequestDto dto)
    {
        this.dto = dto;
        otp = null;
    }

    public SessionUpdateCommand(IRequestDto<string> dto)
    {
        this.dto = dto;
        otp = dto.Payload;
    }

    public class UserTokenHandler : IRequestHandler<SessionUpdateCommand, ResponseDto<AuthenticationResponseDto>>
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

        public async Task<ResponseDto<AuthenticationResponseDto>> Handle(SessionUpdateCommand request, CancellationToken cancellationToken)
        {
            if (currentUserInfo.SessionId is not null)
            {
                var session = await context.UserSessions
                    .Include(x => x.User)
                    .ThenInclude(x => x.Claims)
                    .FirstOrDefaultAsync(x => x.SessionId == currentUserInfo.SessionId && x.TerminationTs == null && x.DeviceId == request.dto.DeviceId, cancellationToken);
                if (session is null) throw new NotAllowedException("Different Device!!! or session expired");

                if (session.OneTimePassword is not null)
                {
                    if (string.IsNullOrWhiteSpace(request.otp) || request.otp != session.OneTimePassword)
                    {
                        throw new NotAllowedException("Otp Isn't matching the recorded.");
                    }

                    session.OneTimePassword = null;
                    await context.SaveChangesAsync(cancellationToken);
                }

                var ci = new ClaimsIdentity("custom");
                ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Session, session.SessionId.ToString()));

                var employee = await context.Employees.FirstOrDefaultAsync(x => x.UserId == session.UserId, cancellationToken);
                if (employee is not null)
                {
                    ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Territory, employee.TerritoryId.ToString()));
                }

                var customer =
                    await context.Customers.FirstOrDefaultAsync(x => x.UserId == session.UserId, cancellationToken);
                if (customer is not null)
                {
                    ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Country, customer.CountryCode));
                }

                foreach (var c in session?.User.Claims?.Where(x => x.RevokedTs == null)?.ToList())
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

                return ResponseDto.Create(AuthenticationResponseDto.Create(token, session.User.Person?.FirstName + " " + session.User.Person?.LastName));
            }
        
            throw new BadRequestException("Invalid request query parameters.");
        }
    }
}