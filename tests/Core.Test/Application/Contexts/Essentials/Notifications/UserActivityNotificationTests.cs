using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Contexts.Essentials.Notifications;
using WeeControl.Core.Application.Exceptions;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Notifications;

public class UserActivityNotificationTests
{
    [Theory]
    [InlineData("subject", "details", "uri")]
    [InlineData("subject", "", "uri")]
    public async void WhenNotificationWasPublished_ListIsIncreased(string subject, string details, string uri)
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();

        var count1 = await helper.EssentialDb.UserNotifications.CountAsync();

        var n = new UserNotification(seed.User.Username, subject, details, uri);
        await new UserNotification.UserNotificationHandler(helper.EssentialDb).Handle(n, default);

        var count2 = await helper.EssentialDb.UserNotifications.Where(x => x.UserId == seed.userId).CountAsync();

        Assert.Equal(count1 + 1, count2);
    }
    
    [Fact]
    public async void WhenNotificationWasPublishedWithInvalidUsername_NotFoundExceptionGetThrown()
    {
        using var helper = new CoreTestHelper();

        var n = new UserNotification("UsernameNotExist", "subject", "details", "uri");

        await Assert.ThrowsAsync<NotFoundException>(() => 
            new UserNotification.UserNotificationHandler(helper.EssentialDb).Handle(n, default));
    }
}