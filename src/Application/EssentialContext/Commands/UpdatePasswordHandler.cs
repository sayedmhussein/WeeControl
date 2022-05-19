using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.EssentialContext.Commands;

public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand>
{
    private readonly IEssentialDbContext context;
    private readonly ICurrentUserInfo currentUserInfo;
    private readonly IPasswordSecurity passwordSecurity;

    public UpdatePasswordHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo, IPasswordSecurity passwordSecurity)
    {
        this.context = context;
        this.currentUserInfo = currentUserInfo;
        this.passwordSecurity = passwordSecurity;
    }
    
    public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
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