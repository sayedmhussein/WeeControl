using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.ApiApp.Domain.Interfaces;

namespace WeeControl.ApiApp.Infrastructure.Notifications;

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