using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.Server.Domain.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.Server.Infrastructure.NotificationServices
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
