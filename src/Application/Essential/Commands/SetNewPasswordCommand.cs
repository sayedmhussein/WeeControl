using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.Essential.Commands;

public class SetNewPasswordCommand : IRequest
{
    public IRequestDto Request { get; }
    public string OldPassword { get; }
    public string NewPassword { get; }
    
    public SetNewPasswordCommand(IRequestDto dto, string oldPassword, string newPassword)
    {
        Request = dto;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
    
    public class SetNewPasswordHandler : IRequestHandler<SetNewPasswordCommand>
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
    
        public async Task<Unit> Handle(SetNewPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.OldPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                throw new BadRequestException("Invalid old and new password supplied");
            }
            
            var session = await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == currentUserInfo.GetSessionId(), cancellationToken);
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == session.UserId && x.Password == passwordSecurity.Hash(request.OldPassword), cancellationToken);

            if (user is null)
            {
                throw new NotFoundException();
            }

            user.UpdatePassword(passwordSecurity.Hash(request.NewPassword));

            await context.SaveChangesAsync(cancellationToken);
        
            return Unit.Value;
        }
    }
}