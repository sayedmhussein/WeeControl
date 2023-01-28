using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WeeControl.ApiApp.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.Core.Application.Contexts.User.Notifications;

public class UserNotificationNotification : INotification
{
    private readonly string username;
    private readonly string subject;
    private readonly string details;
    private readonly string uri;

    public UserNotificationNotification(string username, string subject, string details, string uri)
    {
        this.username = username;
        this.subject = subject;
        this.details = details;
        this.uri = uri;
    }

    public class UserNotificationHandler : INotificationHandler<UserNotificationNotification>
    {
        private readonly IEssentialDbContext essentialDbContext;

        public UserNotificationHandler(IEssentialDbContext essentialDbContext)
        {
            this.essentialDbContext = essentialDbContext;
        }

        public async Task Handle(UserNotificationNotification notification, CancellationToken cancellationToken)
        {
            var user = await essentialDbContext.Users.FirstOrDefaultAsync(x => x.Username == notification.username, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            await essentialDbContext.UserNotifications.AddAsync(
                new UserNotificationDbo()
                {
                    UserId = user.UserId,
                    Subject = notification.subject,
                    Details = notification.details,
                    Link = notification.uri
                }, cancellationToken);

            await essentialDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}