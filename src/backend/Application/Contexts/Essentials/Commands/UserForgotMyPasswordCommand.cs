using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Contexts.Essentials.Notifications;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel.ExtensionMethods;

namespace WeeControl.Core.Application.Contexts.Essentials.Commands;

public class UserForgotMyPasswordCommand : IRequest
{
    private readonly RequestDto<UserPasswordResetRequestDto> dto;

    public UserForgotMyPasswordCommand(RequestDto<UserPasswordResetRequestDto> dto)
    {
        this.dto = dto;
    }

    public class ForgotMyPasswordHandler : IRequestHandler<UserForgotMyPasswordCommand>
    {
        private readonly IEssentialDbContext context;
        private readonly IMediator mediator;
        private readonly IPasswordSecurity passwordSecurity;

        public ForgotMyPasswordHandler(IEssentialDbContext context, IMediator mediator, IPasswordSecurity passwordSecurity)
        {
            this.context = context;
            this.mediator = mediator;
            this.passwordSecurity = passwordSecurity;
        }

        public async Task Handle(UserForgotMyPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.dto.GetModelValidationErrors().Any() || request.dto.Payload.GetModelValidationErrors().Any())
            {
                throw new BadRequestException("Invalid device or email or username");
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == request.dto.Payload.Username.ToLower() && x.Email == request.dto.Payload.Email.ToLower(), cancellationToken);
            if (user is not null)
            {
                if (string.IsNullOrEmpty(user.SuspendArgs) == false)
                    throw new NotAllowedException("Account is locked.");

                var password = passwordSecurity.GenerateRandomPassword();
                user.SetTemporaryPassword(passwordSecurity.Hash(password));
                await context.SaveChangesAsync(cancellationToken);

                await mediator.Publish(new PasswordResetNotification(user.UserId, password), cancellationToken);

                return;
            }

            //return Unit.Value;
        }
    }
}