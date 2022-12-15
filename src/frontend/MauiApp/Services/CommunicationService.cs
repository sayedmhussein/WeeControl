using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.MauiApp.Services;

public class CommunicationService : ICommunication
{
    public Task SendAnEmail(IEnumerable<string> to, string subject, string body)
    {
        return SendAnEmail(to, subject, body, null);
    }

    public Task SendAnEmail(IEnumerable<string> to, string subject, string body, IEnumerable<string> attachments)
    {
        var message = new EmailMessage
        {
            Subject = subject,
            Body = body,
            BodyFormat = EmailBodyFormat.PlainText,
            To = new List<string>(to)
        };
        
        if (attachments != null && attachments.Any())
        {
            foreach (var path in attachments)
            {
                message.Attachments?.Add(new EmailAttachment(path));
            }
        }

        return Email.Default.ComposeAsync(message);
    }

    public Task SendSms(IEnumerable<string> to, string text)
    {
        var message = new SmsMessage(text, to);

        return Sms.Default.ComposeAsync(message);
    }

    public Task<bool> IsConnectedToInternet()
    {
        var accessType = Connectivity.Current.NetworkAccess;
        return Task.FromResult(accessType == NetworkAccess.Internet);
    }

    public Task<bool> PhoneDial(string phoneNo)
    {
        if (PhoneDialer.Default.IsSupported)
        {
            PhoneDialer.Default.Open("000-000-0000");
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}