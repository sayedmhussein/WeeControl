using WeeControl.Core.Application.Contexts.Essentials.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.Domain.Contexts.Essentials;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Commands;

public class SessionTerminateCommand1Tests
{
    [Fact]
    public async void WhenSessionExistAndNotTerminated_SessionBecomeTerminated()
    {
        using var testHelper = new CoreTestHelper();
        var session = UserSessionDbo.Create(GetUserId(testHelper), "device", "0000");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        //
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);
        //
        var handler = new SessionTerminateCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        var dto = RequestDto.Create("device", null, null);
        await handler.Handle(new SessionTerminateCommand(dto), default);

        Assert.NotNull(testHelper.EssentialDb.UserSessions.First(x => x.SessionId == session.SessionId).TerminationTs);
    }

    [Fact]
    public async void WhenRequestDtoHasDifferentSession_ThrowNotAllowedException()
    {
        using var testHelper = new CoreTestHelper();
        var session = UserSessionDbo.Create(GetUserId(testHelper), "device", "0000");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(Guid.NewGuid());

        var handler = new SessionTerminateCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new SessionTerminateCommand(RequestDto.Create("device", 0, 0)), default));
    }

    [Fact]
    public async void WhenRequestDtoHasDifferentDevice_ThrowNotAllowedException()
    {
        using var testHelper = new CoreTestHelper();
        var session = UserSessionDbo.Create(GetUserId(testHelper), "device", "0000");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var handler = new SessionTerminateCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new SessionTerminateCommand(RequestDto.Create("Another device", 0, 0)), default));
    }

    [Fact]
    public async void WhenSessionAlreadyTerminated_ThrowNotAllowedException()
    {
        using var testHelper = new CoreTestHelper();
        var session = UserSessionDbo.Create(GetUserId(testHelper), "device", "0000");
        session.TerminationTs = DateTime.UtcNow;
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var handler = new SessionTerminateCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new SessionTerminateCommand(RequestDto.Create("device", 0, 0)), default));
    }

    [Fact]
    public async void WhenDeviceIDNotSupplied_ThrowBadRequestException()
    {
        using var testHelper = new CoreTestHelper();
        var session = UserSessionDbo.Create(GetUserId(testHelper), "device", "0000");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        //
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);
        //
        var handler = new SessionTerminateCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        await Assert.ThrowsAsync<NotAllowedException>(() =>
            handler.Handle(new SessionTerminateCommand(RequestDto.Create(string.Empty, 0, 0)), default));
    }

    [Fact]
    public async void WhenSessionIDIsNull_ThrowArgumentNullException()
    {
        using var testHelper = new CoreTestHelper();
        var session = UserSessionDbo.Create(GetUserId(testHelper), "device", "0000");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        //
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns((Guid?)null);
        //
        var handler = new SessionTerminateCommand.LogoutHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            handler.Handle(new SessionTerminateCommand(RequestDto.Create("device", 0, 0)), default));
    }

    private Guid GetUserId(CoreTestHelper helper)
    {
        var domain = helper.SeedDatabase();
        var user = helper.EssentialDb.Users.FirstOrDefault(x => x.Username == domain.User.Username);
        return user.UserId;
    }
}