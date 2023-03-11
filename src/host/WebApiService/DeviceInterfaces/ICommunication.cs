namespace WeeControl.Host.WebApiService.DeviceInterfaces;

public interface ICommunication
{
    string ServerUrl { get; }

    HttpClient HttpClient { get; }

    Task SendAnEmail(IEnumerable<string> to, string subject, string body);

    Task SendAnEmail(IEnumerable<string> to, string subject, string body, IEnumerable<string> attachments);

    Task SendSms(IEnumerable<string> to, string text);

    Task<bool> IsConnectedToInternet();

    Task<bool> PhoneDial(string phoneNo);
}