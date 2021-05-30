using System;
using MySystem.Infrastructure.NotificationService;
using Xunit;
namespace MySystem.Infrastructure.Test.NotificationService
{
    public class EmailServiceTests
    {
        [Fact(Skip = "Require External communications")]
        public async void WhenSendingAnEmailToDeveloper_DeveloperShouldReceiveTheEmail()
        {
            var connString = "host=mail.gmx.com;port=587;useSSL=true;username=sayed.hussein@gmx.com;password=";

            await new EmailService(connString).SendAsync("sayed.hussein@gmx.com", "sayed.otis@gmail.com", "Test Message", "This is test Message");
        }
    }
}
