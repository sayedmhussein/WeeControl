using System;
using System.Linq;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.Domain.Contexts.Essential;
using Xunit;

namespace WeeControl.Application.Test.Essential.Queries;

public class NotificationQueryTests
{
    [Fact]
    public async void ReturnListOfNotificationsTest()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        testHelper.EssentialDb.Users.Add(user);
        testHelper.EssentialDb.SaveChanges();

        testHelper.EssentialDb.UserSessions.Add(SessionDbo.Create(user.UserId, "device"));
        testHelper.EssentialDb.UserNotifications.Add(NotificationDbo.Create(user.UserId, "1", "1", ""));
        testHelper.EssentialDb.UserNotifications.Add(NotificationDbo.Create(user.UserId, "2", "2", ""));
        testHelper.EssentialDb.UserNotifications.Add(NotificationDbo.Create(user.UserId, "3", "3", ""));
        testHelper.EssentialDb.UserNotifications.Add(NotificationDbo.Create(Guid.NewGuid(), "1", "1", ""));

        testHelper.EssentialDb.SaveChanges();

        testHelper.CurrentUserInfoMock.SetupGet(x => x.SessionId).Returns(testHelper.EssentialDb.UserSessions.First().SessionId);

        var handler = GetHandler(testHelper);

        var list = await handler.Handle(new NotificationQuery(), default);
        
        Assert.Equal(3, list.Payload.Count());
    }
    
    private NotificationQuery.NotificationHandler GetHandler(TestHelper testHelper)
    {
        return new NotificationQuery.NotificationHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);
    }
}