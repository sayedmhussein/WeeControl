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
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();
        
        var dto = await GetDto(testHelper, seed.userId, seed.sessionId);

        Assert.NotEmpty(dto.Payload.FullName);
        Assert.Equal(6, dto.Payload.Notifications.Count());
    }
    
    [Fact]
    public async void WhenNotExist()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();

        await Assert.ThrowsAsync<NotFoundException>(() => GetDto(testHelper, seed.userId, Guid.NewGuid()));
    }

    private Task<ResponseDto<HomeResponseDto>> GetDto(CoreTestHelper coreTestHelper, Guid userId, Guid sessionId)
    {
        coreTestHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(userId, "1", "1", ""));
        coreTestHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(userId, "2", "2", ""));
        coreTestHelper.EssentialDb.UserNotifications.Add(UserNotificationDbo.Create(userId, "3", "3", ""));
        coreTestHelper.EssentialDb.SaveChanges();
        
        coreTestHelper.CurrentUserInfoMock.SetupGet(x => x.SessionId).Returns(sessionId);
        var handler = new HomeQuery.HomeHandler(coreTestHelper.EssentialDb, coreTestHelper.CurrentUserInfoMock.Object);
        return handler.Handle(new HomeQuery(), default);
    }
}