﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Infrastructure.Notifications
{
    public class SmsService : ISmsNotificationService
    {
        public SmsService()
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