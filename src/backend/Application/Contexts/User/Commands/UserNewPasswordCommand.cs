using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.DataTransferObject;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.Application.Contexts.User.Commands;

public class UserNewPasswordCommand : IRequest
{
    private RequestDto Request { get; }
    private string OldPassword { get; }
    private string NewPassword { get; }

    public UserNewPasswordCommand(RequestDto dto, string oldPassword, string newPassword)
    {
        Request = dto;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    public class SetNewPasswordHandler : IRequestHandler<UserNewPasswordCommand>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo currentUserInfo;
        private readonly IPasswordSecurity passwordSecurity;

        public SetNewPasswordHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo, IPasswordSecurity passwordSecurity)
        {
            this.context = context;
            this.currentUserInfo = currentUserInfo;
            this.passwordSecurity = passwordSecurity;
        }

        public async Task<Unit> Handle(UserNewPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.OldPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                throw new BadRequestException("Invalid old and new password supplied");
            }

            var session = await context.UserSessions.FirstOrDefaultAsync(x => x.SessionId == currentUserInfo.SessionId, cancellationToken);
            var user = await context.Users.FirstOrDefaultAsync(x =>
                x.UserId == session.UserId &&
                (x.Password == passwordSecurity.Hash(request.OldPassword) || x.TempPassword == passwordSecurity.Hash(request.OldPassword)), cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            user.UpdatePassword(passwordSecurity.Hash(request.NewPassword));

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}