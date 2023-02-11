using System;
using WeeControl.Core.Application.Contexts.User.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.Domain.Contexts.User;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Commands;

public class SessionUpdateCommandTests
{
    [Fact]
    public async void WhenOtpIsRequired_ThrowsNotAllowedException()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = new UserSessionDbo(user.UserId, "device", "0000");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var handler = GetHandler(testHelper);
        var query = GetQuery("device", null);

        await Assert.ThrowsAsync<NotAllowedException>(() => handler.Handle(query, default));
    }

    #region Using SessionId
    [Fact]
    public async void WhenSessionIsActive_ReturnToken()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = new UserSessionDbo(user.UserId, "device", null);
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var response = await GetHandler(testHelper).Handle(GetQuery("device", null), default);

        Assert.NotEmpty(response.Payload.Token);
        Assert.NotEmpty(response.Payload.FullName);
    }

    [Fact]
    public async void WhenSessionIsTerminated_ThrowNotAllowedException()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        user.SetTemporaryPassword(testHelper.PasswordSecurity.Hash("temporary"));
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = new UserSessionDbo(user.UserId, "device", null);
        session.TerminationTs = DateTime.UtcNow;
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var query = GetQuery("device", null);
        await Assert.ThrowsAsync<NotAllowedException>(() => GetHandler(testHelper).Handle(query, default));
    }

    [Fact]
    public async void WhenSessionIsActiveButFromDifferentDevice_ThrowNotAllowedException()
    {
        using var testHelper = new TestHelper();
        var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
        user.SetTemporaryPassword(testHelper.PasswordSecurity.Hash("temporary"));
        await testHelper.EssentialDb.Users.AddAsync(user);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var session = new UserSessionDbo(user.UserId, "device", null);
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var query = GetQuery("device2", null);
        await Assert.ThrowsAsync<NotAllowedException>(() => GetHandler(testHelper).Handle(query, default));
    }
    #endregion

    private SessionUpdateCommand.UserTokenHandler GetHandler(TestHelper testHelper)
    {
        testHelper.ConfigurationMock.Setup(x => x["Jwt:Key"]).Returns(new string('a', 30));
        return new SessionUpdateCommand.UserTokenHandler(
            testHelper.EssentialDb,
            testHelper.JwtService,
            testHelper.MediatorMock.Object,
            testHelper.ConfigurationMock.Object,
            testHelper.CurrentUserInfoMock.Object,
            testHelper.PasswordSecurity);
    }

    private SessionUpdateCommand GetQuery(string device, string otp)
    {
        return otp is null
            ? new SessionUpdateCommand(RequestDto.Create(device, 0, 0)) :
            new SessionUpdateCommand(RequestDto.Create(otp, device, 0, 0));
    }
}