using WeeControl.ApiApp.Domain.Interfaces;

namespace WeeControl.ApiApp.Domain.Models;

public class MessageDto : IMessageDto
{
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}