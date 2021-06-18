using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;

namespace WeeControl.Server.Domain.Models
{
    public class MessageDto : IMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
