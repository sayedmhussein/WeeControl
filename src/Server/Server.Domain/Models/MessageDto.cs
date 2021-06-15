using System;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Domain.Models
{
    public class MessageDto : IMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
