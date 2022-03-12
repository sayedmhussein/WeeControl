using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.Credentials;
using WeeControl.Backend.Domain.Credentials.DatabaseObjects;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.RequestsResponses;

namespace WeeControl.Backend.Application.BoundContexts.Credentials.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, TokenDto>
    {
        private readonly ICredentialsDbContext context;

        public RegisterHandler(ICredentialsDbContext context)
        {
            this.context = context;
        }

        public async Task<TokenDto> Handle(RegisterCommand cmd, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(cmd.Request.DeviceId))
            {
                throw new BadRequestException("Invalid Device");
            }

            else if (string.IsNullOrWhiteSpace(cmd.Payload.Password) ||
                (string.IsNullOrWhiteSpace(cmd.Payload.Username) && string.IsNullOrWhiteSpace(cmd.Payload.Email)))
            {
                throw new ValidationException();
            }

            if (context.Users.Where(x =>
            (string.IsNullOrWhiteSpace(cmd.Payload.Username) == false && x.Username == cmd.Payload.Username) ||
            (string.IsNullOrWhiteSpace(cmd.Payload.Username) && x.Email == cmd.Payload.Email)).Any())
            {
                throw new ConflictFailureException();
            }

            await context.Users.AddAsync(new UserDbo()
            {
                Email = cmd.Payload.Email,
                Username = cmd.Payload.Username,
                Password = cmd.Payload.Password
            }, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            var response = new TokenDto() { Token = "Somestring" };
            return response;
        }
    }
}
