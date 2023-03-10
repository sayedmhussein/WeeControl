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
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Commands;

public class SessionUpdateCommand : IRequest<ResponseDto<TokenResponseDto>>
{
    private readonly RequestDto dto;
    private readonly string otp;

    public SessionUpdateCommand(RequestDto dto)
    {
        this.dto = dto;
        otp = null;
    }

    public SessionUpdateCommand(RequestDto dto, string otp)
    {
        this.dto = dto;
        this.otp = otp;
    }

    public class UserTokenHandler : IRequestHandler<SessionUpdateCommand, ResponseDto<TokenResponseDto>>
    {
        private readonly IConfiguration configuration;
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo currentUserInfo;
        private readonly IJwtService jwtService;
        private readonly IMediator mediator;
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

        public async Task<ResponseDto<TokenResponseDto>> Handle(SessionUpdateCommand request,
            CancellationToken cancellationToken)
        {
            if (currentUserInfo.SessionId is not null)
            {
                var session = await context.UserSessions
                    .Include(x => x.User)
                    .ThenInclude(x => x.Claims)
                    .FirstOrDefaultAsync(
                        x => x.SessionId == currentUserInfo.SessionId && x.TerminationTs == null &&
                             x.DeviceId == request.dto.DeviceId, cancellationToken);
                if (session is null) throw new NotAllowedException("Different Device!!! or session expired");

                if (session.OneTimePassword is not null)
                {
                    if (string.IsNullOrWhiteSpace(request.otp) || request.otp != session.OneTimePassword)
                        throw new NotAllowedException("Otp Isn't matching the recorded.");

                    session.DisableOtpRequirement();
                    await context.SaveChangesAsync(cancellationToken);
                }

                var ci = new ClaimsIdentity("custom");
                ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Session, session.SessionId.ToString()));

                var employee =
                    await context.Employees.FirstOrDefaultAsync(x => x.PersonId == session.UserId, cancellationToken);
                if (employee is not null)
                    ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.CustomerTerritory, Guid.Empty.ToString()));

                var customer =
                    await context.Customers.FirstOrDefaultAsync(x => x.UserId == session.UserId, cancellationToken);
                if (customer is not null) ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Country, customer.CountryCode));

                foreach (var c in session?.User.Claims?.Where(x => x.RevokedTs == null)?.ToList())
                    ci.AddClaim(new Claim(c.ClaimType, c.ClaimValue));

                if (configuration["Jwt:Key"] is null) throw new NullReferenceException();

                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

                var descriptor = new SecurityTokenDescriptor
                {
                    Subject = ci,
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddDays(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = jwtService.GenerateToken(descriptor);

                return ResponseDto.Create(TokenResponseDto.Create(token));
            }

            throw new BadRequestException("Invalid request query parameters.");
        }
    }
}