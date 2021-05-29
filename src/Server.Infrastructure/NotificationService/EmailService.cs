using System;
using System.Threading.Tasks;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.NotificationModel;

namespace MySystem.Infrastructure.NotificationService
{
    public class EmailService : INotificationService
    {
        public EmailService()
        {
        }

        public Task SendAsync(MessageDto message)
        {
            return Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
