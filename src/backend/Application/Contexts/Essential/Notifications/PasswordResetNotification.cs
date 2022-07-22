using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Interfaces;
using WeeControl.Domain.Models;

namespace WeeControl.Application.Contexts.Essential.Notifications;

public class PasswordResetNotification : INotification
{
    private readonly Guid userId;
    private readonly string newPassword;

    public PasswordResetNotification(Guid userid, string newPassword)
    {
        this.userId = userid;
        this.newPassword = newPassword;
    }
    
    public class PasswordResetHandler : INotificationHandler<PasswordResetNotification>
    {
        private readonly IEmailNotificationService notification;
        private readonly IEssentialDbContext context;

        public PasswordResetHandler(IEmailNotificationService notification, IEssentialDbContext context)
        {
            this.notification = notification;
            this.context = context;
        }
        
        public async Task Handle(PasswordResetNotification notif, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == notif.userId, cancellationToken);
            if (user == null)
            {
                throw new NullReferenceException();
            }

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
    
    private static IMessageDto GetMessage(string to, string newPassword)
    {
        return new MessageDto()
        {
            To = to,
            Subject = "WeeControl - A New Password Has Been Created for You",
            Body = "Your new password is: " + newPassword
        };
    }
}