using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Notifications;

public class UserSessionLog : INotification
{
    private readonly string logContext;
    private readonly string logDetails;

    public UserSessionLog(string logContext, string logDetails)
    {
        this.logContext = logContext;
        this.logDetails = logDetails;
    }

    public class UserActivityNotificationHandler : INotificationHandler<UserSessionLog>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo currentUserInfo;

        public UserActivityNotificationHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo)
        {
            this.context = context;
            this.currentUserInfo = currentUserInfo;
        }

        public async Task Handle(UserSessionLog notif, CancellationToken cancellationToken)
        {
            try
            {
                var id = currentUserInfo.SessionId;
                if (id is null)
                {
                    return;
                }

                var session = await context.UserSessions.FirstOrDefaultAsync(x => x.SessionId == id, cancellationToken);
                if (session is not null)
                {
                    var log = session.CreateLog(notif.logContext, notif.logDetails);

                    await context.SessionLogs.AddAsync(log, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}