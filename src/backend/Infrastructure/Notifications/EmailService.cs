using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Security;
using WeeControl.Core.Application.Contexts;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.ApiApp.Infrastructure.Notifications;

public class EmailService : IEmailNotificationService
{
    private readonly string email;
    private readonly string host;
    private readonly string password;
    private readonly int port;
    private readonly string username;
    private readonly bool useSsl;

    public EmailService(string configurationString)
    {
        var args = configurationString.Split(";");
        foreach (var arg in args)
        {
            var item = arg.Split("=");
            try
            {
                switch (item[0].Trim().ToLower())
                {
                    case "host":
                        host = item[1].Trim();
                        break;
                    case "port":
                        port = short.Parse(item[1].Trim());
                        break;
                    case "useSSL":
                        useSsl = item[1].Trim() == "true";
                        break;
                    case "username":
                        username = item[1].Trim();
                        break;
                    case "password":
                        password = item[1].Trim();
                        break;
                    case "name":
                        password = item[1].Trim();
                        break;
                    case "email":
                        email = item[1].Trim();
                        break;
                }
            }
            catch
            {
                throw new InvalidParameterException("Connection string is invalid.");
            }
        }
    }

    public Task SendAsync(MessageDto message)
    {
        return SendAsync(message.From, message.To, message.Subject, message.Body);
    }

    public async Task SendAsync(IEnumerable<MessageDto> messages)
    {
        foreach (var message in messages) await SendAsync(message);
    }

    public async Task SendAsync(string from, string to, string subject, string body)
    {
        var msg = new MimeMessage();
        msg.From.Add(MailboxAddress.Parse(from ?? email));
        msg.To.Add(MailboxAddress.Parse(to));
        msg.Subject = subject;
        msg.Body = new TextPart(TextFormat.Plain) {Text = body};

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, useSsl);

        // Note: only needed if the SMTP server requires authentication
        if (string.IsNullOrEmpty(username) == false && string.IsNullOrEmpty(password) == false)
            await client.AuthenticateAsync(username, password);

        await client.SendAsync(msg);
        await client.DisconnectAsync(true);
    }
}