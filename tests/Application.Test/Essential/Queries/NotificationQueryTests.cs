using System;
using System.Linq;
using WeeControl.Core.Domain.Contexts.User;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Queries;

public class NotificationQueryTests
{
    [Fact]
    public void ReturnListOfNotificationsTest()
    {
        using var testHelper = new TestHelper();
        var seed = testHelper.SeedDatabase();
        testHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(seed.userId, "1", "1", ""));
        testHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(seed.userId, "2", "2", ""));
        testHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(seed.userId, "3", "3", ""));
        testHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(Guid.NewGuid(), "1", "1", ""));
        //
        testHelper.EssentialDb.SaveChanges();

        testHelper.CurrentUserInfoMock.SetupGet(x => x.SessionId).Returns(seed.sessionId);

        //var handler = GetHandler(testHelper);

        // var list = await handler.Handle(new NotificationQuery(), default);
        //
        // Assert.Equal(3, list.Payload.Count());
    }

    // private NotificationQuery.NotificationHandler GetHandler(TestHelper testHelper)
    // {
    //     return new NotificationQuery.NotificationHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);
    // }
}