namespace WeeControl.ApiApp.Domain.Interfaces;

public interface IMessageDto
{
    string From { get; set; }
    string To { get; set; }
    string Subject { get; set; }
    string Body { get; set; }
}