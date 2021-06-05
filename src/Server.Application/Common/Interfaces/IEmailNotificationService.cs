using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySystem.Application.Models;

namespace MySystem.Application.Common.Interfaces
{
    public interface IEmailNotificationService
    {
        Task SendAsync(MessageDto message);
        Task SendAsync(IEnumerable<MessageDto> messages);
    }
}
