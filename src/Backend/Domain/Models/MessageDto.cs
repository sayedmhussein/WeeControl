using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Domain.Models
{
    public class MessageDto : IMessageDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
