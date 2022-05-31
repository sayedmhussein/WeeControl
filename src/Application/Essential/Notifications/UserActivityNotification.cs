using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;

namespace WeeControl.Application.Essential.Notifications;

public class UserActivityNotification : INotification
{
    private readonly string logContext;
    private readonly string logDetails;

    public UserActivityNotification(string logContext, string logDetails)
    {
        this.logContext = logContext;
        this.logDetails = logDetails;
    }
    
    public class UserActivityNotificationHandler : INotificationHandler<UserActivityNotification>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo currentUserInfo;

        public UserActivityNotificationHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo)
        {
            this.context = context;
            this.currentUserInfo = currentUserInfo;
        }

        public async Task Handle(UserActivityNotification notif, CancellationToken cancellationToken)
        {
            var id = currentUserInfo.GetSessionId();
            if (id is null)
            {
                return;    
            }
            
            var session = await context.Sessions.FirstAsync(x => x.SessionId == id, cancellationToken);
            var log = session.CreateLog(notif.logContext, notif.logDetails);
            
            await context.Logs.AddAsync(log, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}