using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySystem.Application.Common.Interfaces;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Infrastructure.NotificationServices
{
    public class SmsService : ISmsNotificationService
    {
        public SmsService()
        {
        }

        public Task SendAsync(IMessage message)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(IEnumerable<IMessage> messages)
        {
            throw new NotImplementedException();
        }
    }
}
