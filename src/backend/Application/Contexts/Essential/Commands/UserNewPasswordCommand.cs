using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.ApiApp.Application.Exceptions;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.ApiApp.Application.Contexts.Essential.Commands;

public class UserNewPasswordCommand : IRequest
{
    private IRequestDto Request { get; }
    private string OldPassword { get; }
    private string NewPassword { get; }
    
    public UserNewPasswordCommand(IRequestDto dto, string oldPassword, string newPassword)
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