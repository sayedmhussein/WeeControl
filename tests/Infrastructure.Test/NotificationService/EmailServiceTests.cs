using WeeControl.Backend.Infrastructure.Notifications;
using Xunit;

namespace WeeControl.Server.Infrastructure.Test.NotificationService;

public class EmailServiceTests
{
    [Fact(Skip = "Require External communications")]
    public async void WhenSendingAnEmailToDeveloper_DeveloperShouldReceiveTheEmail()
    {
        var connString =
            "host=mail.gmx.com;port=587;useSSL=true;username=sayed.hussein@gmx.com;password=, name=Sayed Hussein, email=sayed.hussein@gmx.com";

        await new EmailService(connString).SendAsync("sayed.hussein@gmx.com", "sayed.hussein@gmx.com", "Test Message",
            "This is test Message");
    }
}