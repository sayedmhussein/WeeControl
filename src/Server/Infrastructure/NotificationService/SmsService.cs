﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;

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
