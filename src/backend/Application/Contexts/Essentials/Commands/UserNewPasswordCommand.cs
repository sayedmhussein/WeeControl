using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel.DtoParent;

namespace WeeControl.Core.Application.Contexts.Essentials.Commands;

public class UserNewPasswordCommand : IRequest
{
    public UserNewPasswordCommand(RequestDto dto, string oldPassword, string newPassword)
    {
        Request = dto;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }

    private RequestDto Request { get; }
    private string OldPassword { get; }
    private string NewPassword { get; }

    public class SetNewPasswordHandler : IRequestHandler<UserNewPasswordCommand>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo currentUserInfo;
        private readonly IPasswordSecurity passwordSecurity;

        public SetNewPasswordHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo,
            IPasswordSecurity passwordSecurity)
        {
            this.context = context;
            this.currentUserInfo = currentUserInfo;
            this.passwordSecurity = passwordSecurity;
        }

        public async Task Handle(UserNewPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.OldPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
                throw new BadRequestException("Invalid old and new password supplied");

            var session = await context.UserSessions
                .Include(x => x.User)
                .Where(x => x.SessionId == currentUserInfo.SessionId)
                .Where(x =>
                    x.User.Password == passwordSecurity.Hash(request.OldPassword) ||
                    x.User.TempPassword == passwordSecurity.Hash(request.OldPassword))
                .FirstOrDefaultAsync(cancellationToken);

            if (session is null) throw new NotFoundException("User not found!");

            session.User.UpdatePassword(passwordSecurity.Hash(request.NewPassword));
            context.Users.Attach(session.User);
            context.Users.Entry(session.User).Property(x => x.Password).IsModified = true;

            await context.SaveChangesAsync(cancellationToken);

            //return Unit.Value;
        }
    }
}