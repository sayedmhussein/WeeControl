using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Commands;

public class LogoutCommand : IRequest
{
    private readonly RequestDto request;
    
    public LogoutCommand(RequestDto request)
    {
        this.request = request;
    }

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
                         x.DeviceId == request.request.DeviceId && 
                         x.TerminationTs == null, cancellationToken);

            if (session is not null)
            {
                session.TerminationTs = DateTime.UtcNow;
                await context.Logs.AddAsync(session.CreateLog("Logout","Terminating session id:" + session.SessionId), cancellationToken);
            }
            else
            {
                throw new NotAllowedException("Already logged out!");
            }
        
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}