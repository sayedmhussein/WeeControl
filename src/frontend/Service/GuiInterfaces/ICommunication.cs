namespace WeeControl.Frontend.AppService.GuiInterfaces;

public interface ICommunication
{
    string ServerUrl { get; }
    
    HttpClient HttpClient { get; set; }

    Task SendAnEmail(IEnumerable<string> to, string subject, string body);
    
    Task SendAnEmail(IEnumerable<string> to, string subject, string body, IEnumerable<string> attachments);

    Task SendSms(IEnumerable<string> to, string text);

    Task<bool> IsConnectedToInternet();

    Task<bool> PhoneDial(string phoneNo);
}