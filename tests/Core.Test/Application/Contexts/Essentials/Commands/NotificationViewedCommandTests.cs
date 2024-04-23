using MediatR;
using Moq;
using WeeControl.Core.Application.Contexts.Essentials.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Commands;

public class NotificationViewedCommandTests
{
    private readonly Mock<IMediator> mediatorMock = new();

    [Fact]
    public async void WhenNotificationGetViewedBySameUser()
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();

        var notification = helper.EssentialDb.UserNotifications.First(x => x.UserId == seed.personId);

        Assert.Null(helper.EssentialDb.UserNotifications.First(x => x.NotificationId == notification.NotificationId)
            .ReadTs);

        await new NotificationViewedCommand.NotificationViewedHandler(helper.EssentialDb,
                GetCurrentUserInfo(seed.personId), mediatorMock.Object)
            .Handle(new NotificationViewedCommand(notification.NotificationId), default);

        Assert.NotNull(helper.EssentialDb.UserNotifications.First(x => x.NotificationId == notification.NotificationId)
            .ReadTs);
    }

    [Fact]
    public async void WhenNotificationAlreadyGetViewedBySameUser_ThrowNotAllowedException()
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();

        var notification = helper.EssentialDb.UserNotifications.First(x => x.UserId == seed.personId);

        await new NotificationViewedCommand.NotificationViewedHandler(helper.EssentialDb,
                GetCurrentUserInfo(seed.personId), mediatorMock.Object)
            .Handle(new NotificationViewedCommand(notification.NotificationId), default);


        await Assert.ThrowsAsync<NotAllowedException>(() =>
            new NotificationViewedCommand.NotificationViewedHandler(helper.EssentialDb, GetCurrentUserInfo(seed.personId),
                    mediatorMock.Object)
                .Handle(new NotificationViewedCommand(notification.NotificationId), default));
    }

    [Fact]
    public async void WhenNotificationGetViewedDifferentUser_ThrowNotAllowedException()
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();
        var other = helper.SeedDatabase("AnotherUsername");


        var notification = helper.EssentialDb.UserNotifications.First(x => x.UserId == seed.personId);

        await Assert.ThrowsAsync<NotAllowedException>(() =>
            new NotificationViewedCommand.NotificationViewedHandler(helper.EssentialDb,
                    GetCurrentUserInfo(other.personId), mediatorMock.Object)
                .Handle(new NotificationViewedCommand(notification.NotificationId), default));
    }

    [Fact]
    public async void WhenInvalidNotificationGetViewedBySameUser_ThrowNotFoundException()
    {
        using var helper = new CoreTestHelper();
        var seed = helper.SeedDatabase();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            new NotificationViewedCommand.NotificationViewedHandler(helper.EssentialDb, GetCurrentUserInfo(seed.personId),
                    mediatorMock.Object)
                .Handle(new NotificationViewedCommand(Guid.Empty), default));
    }

    private ICurrentUserInfo GetCurrentUserInfo(Guid sessionId)
    {
        var mock = new Mock<ICurrentUserInfo>();
        mock.Setup(x => x.SessionId).Returns(sessionId);
        mediatorMock.Setup(x => x.Send(It.IsAny<IRequest<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sessionId);

        return mock.Object;
    }
}