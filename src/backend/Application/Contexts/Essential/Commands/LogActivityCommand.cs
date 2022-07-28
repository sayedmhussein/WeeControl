using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;

namespace WeeControl.Application.Contexts.Essential.Commands;

public class LogActivityCommand : IRequest
{
    public string LogContext { get; }
    public string LogDetails { get; }
    public Guid SessionId { get; }

    public LogActivityCommand(string context, string details, Guid? sessionId)
    {
        LogContext = context;
        LogDetails = details;
        SessionId = sessionId ?? throw new ArgumentNullException();
    }
    
    public class LogActivityHandler : IRequestHandler<LogActivityCommand>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo currentUserInfo;

        public LogActivityHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo)
        {
            this.context = context;
            this.currentUserInfo = currentUserInfo;
        }
    
        public async Task<Unit> Handle(LogActivityCommand request, CancellationToken cancellationToken)
        {
            var id = currentUserInfo.GetSessionId() ?? throw new NullReferenceException("User from IUserInfo can't be null!");
            var session = await context.UserSessions.FirstOrDefaultAsync(x => x.SessionId == id, cancellationToken);
            if (session == null)
            {
                // Here to log this issue in system!
            }

            await context.SessionLogs.AddAsync(session.CreateLog(request.LogContext, request.LogDetails), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        
            return Unit.Value;
        }
    }
}