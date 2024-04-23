using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Notifications;

public class PasswordResetNotification : INotification
{
    private readonly string newPassword;
    private readonly Guid userId;

    public PasswordResetNotification(Guid userid, string newPassword)
    {
        userId = userid;
        this.newPassword = newPassword;
    }

    private static MessageDto GetMessage(string to, string newPassword)
    {
        return new MessageDto
        {
            To = to,
            Subject = "WeeControl - A New Password Has Been Created for You",
            Body = "Your new password is: " + newPassword
        };
    }

    public class PasswordResetHandler : INotificationHandler<PasswordResetNotification>
    {
        private readonly IEssentialDbContext context;
        private readonly IEmailNotificationService notification;

        public PasswordResetHandler(IEmailNotificationService notification, IEssentialDbContext context)
        {
            this.notification = notification;
            this.context = context;
        }

        public async Task Handle(PasswordResetNotification notif, CancellationToken cancellationToken)
        {
            var user = await context.Person.FirstOrDefaultAsync(x => x.PersonId == notif.userId, cancellationToken);
            if (user == null) throw new NullReferenceException();

            var message = GetMessage(user.Email, notif.newPassword);
            Console.WriteLine("From: {0}", message.From);
            Console.WriteLine("To: {0}", message.To);
            Console.WriteLine("Subject: {0}", message.Subject);
            Console.WriteLine("Body: {0}", message.Body);
            try
            {
                await notification.SendAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Notification Service isn't available now!");
                Console.WriteLine(e);
            }
        }
    }
}