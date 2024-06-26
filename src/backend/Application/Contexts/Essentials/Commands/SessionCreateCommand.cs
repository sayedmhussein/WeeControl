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
using WeeControl.Core.Domain.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.DtoParent;
using WeeControl.Core.SharedKernel.ExtensionHelpers;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Commands;

public class SessionCreateCommand(RequestDto<LoginRequestDto> dto) : IRequest<ResponseDto<TokenResponseDto>>
{
    private readonly RequestDto<LoginRequestDto> dto = dto;

    public class SessionCreateHandler(
        IEssentialDbContext context,
        IJwtService jwtService,
        IConfiguration configuration,
        IPasswordSecurity passwordSecurity)
        : IRequestHandler<SessionCreateCommand, ResponseDto<TokenResponseDto>>
    {
        public async Task<ResponseDto<TokenResponseDto>> Handle(SessionCreateCommand request,
            CancellationToken cancellationToken)
        {
            if (request.dto.Payload.GetModelValidationErrors().Any())
                throw new BadRequestException("Invalid request query parameters.");

            var usernameOrEmail = request.dto.Payload.UsernameOrEmail.Trim().ToLower();
            var password = passwordSecurity.Hash(request.dto.Payload.Password);

            var user = await context.Person
                //.Include(x => x.Person)
                .FirstOrDefaultAsync(x =>
                        x.Username == usernameOrEmail || x.Email == usernameOrEmail
                    , cancellationToken);

            if (user is null || (user.Password != password && user.TempPassword != password))
                throw new NotFoundException("User not found!");

            if (user.SuspendArgs is not null) throw new NotAllowedException("Account is locked!");

            if (user.TempPassword == password && user.TempPasswordTs > DateTime.UtcNow.AddMinutes(-10))
            {
                user.UpdatePassword(user.TempPassword);
                user.SetTemporaryPassword(null);
            }

            await context.SaveChangesAsync(cancellationToken);

            var session = await context.UserSessions.FirstOrDefaultAsync(
                x => x.UserId == user.PersonId && x.DeviceId == request.dto.DeviceId && x.TerminationTs == null,
                cancellationToken);
            if (session is null)
            {
                session = UserSessionDbo.Create(user.PersonId, request.dto.DeviceId, "0000");
                await context.UserSessions.AddAsync(session, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                await context.SessionLogs.AddAsync(session.CreateLog("Login", "Created New Session."),
                    cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            var ci = new ClaimsIdentity("custom");
            ci.AddClaim(new Claim(ClaimsValues.ClaimTypes.Session, session.SessionId.ToString()));

            if (configuration["Jwt:Key"] is null) throw new NullReferenceException();

            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = ci,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtService.GenerateToken(descriptor);

            return ResponseDto.Create(TokenResponseDto.Create(token));
        }
    }
}