using WeeControl.Domain.Interfaces;

namespace WeeControl.Domain.Models;

public class MessageDto : IMessageDto
{
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}