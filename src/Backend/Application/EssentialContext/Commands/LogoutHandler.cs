using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Backend.Domain.Databases.Essential;

namespace WeeControl.Backend.Application.EssentialContext.Commands;

public class LogoutHandler : IRequestHandler<LogoutCommand>
{
    private readonly IEssentialDbContext context;
    private readonly ICurrentUserInfo currentUserInfo;

    public LogoutHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo)
    {
        this.context = context;
        this.currentUserInfo = currentUserInfo;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (currentUserInfo.GetSessionId() is null)
        {
            throw new ArgumentNullException(nameof(currentUserInfo));
        }

        var session =
            await context.Sessions.FirstOrDefaultAsync(
                x => x.SessionId == currentUserInfo.GetSessionId() && 
                     x.DeviceId == request.Request.DeviceId && 
                     x.TerminationTs == null, cancellationToken);

        if (session is not null)
        {
            session.TerminationTs = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new NotAllowedException();
        }

        return Unit.Value;
    }
}