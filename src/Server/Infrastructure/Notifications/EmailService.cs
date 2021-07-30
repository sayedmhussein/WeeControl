using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using WeeControl.Server.Domain.Interfaces;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.Server.Infrastructure.Notifications
{
    public class EmailService : IEmailNotificationService
    {
        private readonly string host;
        private readonly int port;
        private readonly bool useSSL;
        private readonly string username;
        private readonly string password;

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
                            useSSL = item[1].Trim() == "true";
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
                            password = item[1].Trim();
                            break;
                    }
                }
                catch
                { }
            }
        }

        public Task SendAsync(IMessage message)
        {
            return SendAsync(message.From, message.To, message.Subject, message.Body);
        }

        public async Task SendAsync(string from, string to, string subject, string body)
        {
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(from));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = body };

            using var client = new SmtpClient();
            client.Connect(host, port, useSSL);

            // Note: only needed if the SMTP server requires authentication
            if (string.IsNullOrEmpty(username) == false && string.IsNullOrEmpty(password) == false)
            {
                client.Authenticate(username, password);
            }

            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }

        public Task SendAsync(IEnumerable<IMessage> messages)
        {
            throw new NotImplementedException();
        }
    }
}
