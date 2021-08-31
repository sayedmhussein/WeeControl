﻿using System;
namespace WeeControl.Backend.Application.Common.Interfaces
{
    public interface IMessage
    {
        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
    }
}