using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.Backend.Domain.Common.Interfaces;

namespace WeeControl.Backend.Infrastructure.Notifications
{
    public class SmsService : ISmsNotificationService
    {
        public SmsService(string connectionString)
        {
        }

        public Task SendAsync(IMessageDto message)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(IEnumerable<IMessageDto> messages)
        {
            throw new NotImplementedException();
        }
    }
}
