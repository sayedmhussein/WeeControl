using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Contexts.Essentials.Queries;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.DtoParent;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Queries;

public class HomeQueryTests
{
    [Fact]
    public async void WhenSuccess_ReturnFullnameAndNotificationsAndFeeds()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();

        var dto = await GetResponseFromHandler(testHelper, seed.userId, seed.sessionId);

        Assert.NotEmpty(dto.Payload.FullName);
        Assert.NotEmpty(dto.Payload.Notifications);
        Assert.NotEmpty(dto.Payload.Feeds);
    }

    [Fact]
    public async void WhenNoNotifications_ReturnUsernameAndEmptyNotifications()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();
        await testHelper.EssentialDb.UserNotifications.ExecuteDeleteAsync();

        var dto = await GetResponseFromHandler(testHelper, seed.userId, seed.sessionId);

        Assert.NotEmpty(dto.Payload.FullName);
        Assert.Empty(dto.Payload.Notifications);
    }

    [Fact]
    public async void WhenUserSessionNotExist_ThrowNotFoundException()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            GetResponseFromHandler(testHelper, seed.userId, Guid.NewGuid()));
    }

    private static Task<ResponseDto<HomeResponseDto>> GetResponseFromHandler(CoreTestHelper coreTestHelper, Guid userId,
        Guid sessionId)
    {
        coreTestHelper.CurrentUserInfoMock.SetupGet(x => x.SessionId).Returns(sessionId);

        var handler = new HomeQuery.HomeHandler(coreTestHelper.EssentialDb, coreTestHelper.CurrentUserInfoMock.Object);
        return handler.Handle(new HomeQuery(), default);
    }
}