using Microsoft.EntityFrameworkCore;
using System.Linq;
using WeeControl.Core.Application.Contexts.User.Notifications;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Notifications;

public class UserActivityNotificationTests
{
    [Fact]
    public async void WhenNotificationWasPublished_ListIsIncreased()
    {
        using var helper = new TestHelper();
        var seed = helper.SeedDatabase();

        var count1 = await helper.EssentialDb.UserNotifications.CountAsync();

        var notif = new UserNotificationNotification(seed.User.Username, "subject", "details", "uri");
        await new UserNotificationNotification.UserNotificationHandler(helper.EssentialDb).Handle(notif, default);

        var count2 = await helper.EssentialDb.UserNotifications.Where(x => x.UserId == seed.userId).CountAsync();

        Assert.Equal(count1 + 1, count2);
    }
}