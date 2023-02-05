using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.Core.Application.Contexts;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.ApiApp.Infrastructure.Notifications;

public class SmsService : ISmsNotificationService
{
    public SmsService(string connectionString)
    {
    }

    public Task SendAsync(MessageDto message)
    {
        throw new NotImplementedException();
    }

    public Task SendAsync(IEnumerable<MessageDto> messages)
    {
        throw new NotImplementedException();
    }
}