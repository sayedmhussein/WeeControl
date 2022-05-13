using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Backend.Domain.Databases.Essential;

namespace WeeControl.Backend.Application.EssentialContext.Commands;

public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand>
{
    private readonly IEssentialDbContext context;
    private readonly ICurrentUserInfo currentUserInfo;

    public UpdatePasswordHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo)
    {
        this.context = context;
        this.currentUserInfo = currentUserInfo;
    }
    
    public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var session = await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == currentUserInfo.GetSessionId(), cancellationToken);
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == session.UserId && x.Password== request.OldPassword, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException();
        }

        user.Password = request.NewPassword;

        await context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}