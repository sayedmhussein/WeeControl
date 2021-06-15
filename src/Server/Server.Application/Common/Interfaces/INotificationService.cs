using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(IMessage message);
        Task SendAsync(IEnumerable<IMessage> messages);
    }
}
