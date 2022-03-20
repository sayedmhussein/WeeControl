using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Backend.Application.BoundContexts.Credentials.Queries;
using WeeControl.Backend.Application.BoundContexts.Shared.Queries;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.Credentials;
using WeeControl.Backend.Domain.Credentials.DatabaseObjects;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;

namespace WeeControl.Backend.Application.BoundContexts.Credentials.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, TokenDto>
    {
        private readonly ICredentialsDbContext context;
        private readonly IMediator mediator;

        public RegisterHandler(ICredentialsDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<TokenDto> Handle(RegisterCommand cmd, CancellationToken cancellationToken)
        {
            await mediator.Send(new VerifyRequestQuery(cmd.Request), cancellationToken);

            if (string.IsNullOrWhiteSpace(cmd.Payload.Password) ||
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

            var user = new UserDbo()
            {
                Email = cmd.Payload.Email,
                Username = cmd.Payload.Username,
                Password = cmd.Payload.Password
            };

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var b= await mediator.Send(new GetNewTokenQuery(cmd.Request, new LoginDto() { Username = user.Username, Password = user.Password }), cancellationToken);
            return b.Payload;
        }
    }
}
