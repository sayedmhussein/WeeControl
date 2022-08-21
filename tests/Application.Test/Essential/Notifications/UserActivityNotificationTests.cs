using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeeControl.ApiApp.Application.Contexts.Essential.Notifications;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Notifications;

public class UserActivityNotificationTests
{
    [Fact]
    public async void WhenNotificationWasPublished_ListIsIncreased()
    {
        using var helper = new TestHelper();
        var user = helper.GetUserDboWithEncryptedPassword("username", "password");
        await helper.EssentialDb.Users.AddAsync(user);
        await helper.EssentialDb.SaveChangesAsync(default);
        
        var count1 = await helper.EssentialDb.UserNotifications.CountAsync();

        var notif = new UserNotificationNotification(user.Username, "subject", "details", "uri");
        await new UserNotificationNotification.UserNotificationHandler(helper.EssentialDb).Handle(notif, default);

        var count2 = await helper.EssentialDb.UserNotifications.Where(x => x.UserId == user.UserId).CountAsync();
        
        Assert.Equal(count1 + 1, count2);
    }
}