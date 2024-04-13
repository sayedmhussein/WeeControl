using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.DataTransferObject.User;

namespace WeeControl.Core.Application.Contexts.User.Commands;

public class SessionCreateCommand : IRequest<ResponseDto<TokenResponseDto>>
{
    private readonly IRequestDto<LoginRequestDto> dto;

    public SessionCreateCommand(IRequestDto<LoginRequestDto> dto)
    {
        this.dto = dto;
    }

    public class SessionCreateHandler : IRequestHandler<SessionCreateCommand, ResponseDto<TokenResponseDto>>
    {
        private readonly IEssentialDbContext context;
        private readonly IJwtService jwtService;
        private readonly IConfiguration configuration;
        private readonly IPasswordSecurity passwordSecurity;

        public SessionCreateHandler(IEssentialDbContext context,
            IJwtService jwtService,
            IConfiguration configuration,
            IPasswordSecurity passwordSecurity)
        {
            this.context = context;
            this.jwtService = jwtService;
            this.configuration = configuration;
            this.passwordSecurity = passwordSecurity;
        }

        public async Task<ResponseDto<TokenResponseDto>> Handle(SessionCreateCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.dto.Payload.UsernameOrEmail) && !string.IsNullOrWhiteSpace(request.dto.Payload.Password))
            {
                var usernameOrEmail = request.dto.Payload.UsernameOrEmail.Trim().ToLower();
                var password = request.dto.Payload.Password;

                var user = await context.Users

                    .Include(x => x.Person)
                    .FirstOrDefaultAsync(x =>
                        (x.Username == usernameOrEmail || x.Email == usernameOrEmail) &&
                        x.Password == passwordSecurity.Hash(password), cancellationToken);

                if (user is null)
                {
                    user = await context.Users
                        .Include(x => x.Person)
                        .FirstOrDefaultAsync(x =>
                                (x.Username == usernameOrEmail || x.Email == usernameOrEmail) &&
                                x.TempPassword == passwordSecurity.Hash(password) && x.TempPasswordTs > DateTime.UtcNow.AddMinutes(-10)
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
                    if (request.dto.Payload.Password == user.TempPassword)
                    {
                        user.UpdatePassword(user.TempPassword);
                    }

                    user.SetTemporaryPassword(null);
                }


                await context.SaveChangesAsync(cancellationToken);

                var session = await context.UserSessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.DeviceId == request.dto.DeviceId && x.TerminationTs == null, cancellationToken);
                if (session is null)
                {
                    session = UserSessionDbo.Create(user.UserId, request.dto.DeviceId, "0000");
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

                return ResponseDto.Create(TokenResponseDto.Create(token, user.Person?.FirstName + " " + user.Person?.LastName));
            }

            throw new BadRequestException("Invalid request query parameters.");
        }
    }
}