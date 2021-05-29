using System;
using System.Threading.Tasks;
using MySystem.Application.NotificationModel;

namespace MySystem.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(MessageDto message);
    }
}
