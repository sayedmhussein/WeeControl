using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Domain.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Notifications;

public class UserNotification : INotification
{
    private readonly string username;
    private readonly string subject;
    private readonly string details;
    private readonly string uri;

    public UserNotification(string username, string subject, string details, string uri)
    {
        this.username = username;
        this.subject = subject;
        this.details = details;
        this.uri = uri;
    }

    public class UserNotificationHandler : INotificationHandler<UserNotification>
    {
        private readonly IEssentialDbContext essentialDbContext;

        public UserNotificationHandler(IEssentialDbContext essentialDbContext)
        {
            this.essentialDbContext = essentialDbContext;
        }

        public async Task Handle(UserNotification notification, CancellationToken cancellationToken)
        {
            var user = await essentialDbContext.Users.FirstOrDefaultAsync(x => x.Username == notification.username, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            await essentialDbContext.UserNotifications.AddAsync(
                UserNotificationDbo
                    .Create(user.UserId, notification.subject, notification.details, notification.uri), 
                cancellationToken);

            await essentialDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}