﻿using System;
namespace MySystem.Application.Models
{
    [Obsolete]
    public class MessageDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
