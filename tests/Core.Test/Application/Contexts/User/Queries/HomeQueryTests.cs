using WeeControl.Core.Application.Contexts.User.Queries;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.Domain.Contexts.User;

namespace WeeControl.Core.Test.Application.Contexts.User.Queries;

public class HomeQueryTests
{
    [Fact]
    public async void ReturnHomeDataAsStoredInDb()
    {
        using var testHelper = new TestHelper();
        var seed = testHelper.SeedDatabase();
        
        var dto = await GetDto(testHelper, seed.userId, seed.sessionId);

        Assert.NotEmpty(dto.Payload.FullName);
        Assert.Equal(3, dto.Payload.Notifications.Count());
    }
    
    [Fact]
    public async void WhenNotExist()
    {
        using var testHelper = new TestHelper();
        var seed = testHelper.SeedDatabase();

        await Assert.ThrowsAsync<NotFoundException>(() => GetDto(testHelper, seed.userId, Guid.NewGuid()));
    }

    private Task<ResponseDto<HomeResponseDto>> GetDto(TestHelper testHelper, Guid userId, Guid sessionId)
    {
        testHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(userId, "1", "1", ""));
        testHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(userId, "2", "2", ""));
        testHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(userId, "3", "3", ""));
        testHelper.EssentialDb.SaveChanges();
        
        testHelper.CurrentUserInfoMock.SetupGet(x => x.SessionId).Returns(sessionId);
        var handler = new HomeQuery.HomeHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);
        return handler.Handle(new HomeQuery(), default);
    }
}