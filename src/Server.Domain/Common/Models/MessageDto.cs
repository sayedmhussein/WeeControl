using WeeControl.Server.Domain.Common.Interfaces;

namespace WeeControl.Server.Domain.Common.Models
{
    public class MessageDto : IMessageDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
