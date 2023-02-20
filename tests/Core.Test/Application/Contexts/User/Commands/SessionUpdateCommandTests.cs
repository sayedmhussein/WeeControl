using WeeControl.Core.Application.Contexts.User.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.Domain.Contexts.User;

namespace WeeControl.Core.Test.Application.Contexts.User.Commands;

public class SessionUpdateCommandTests
{
    [Fact]
    public async void WhenOtpIsRequired_ThrowsNotAllowedException()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();
        var session = UserSessionDbo.Create(seed.userId, "device", "0000");
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
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();
        var session = UserSessionDbo.Create(seed.userId, "device", "0000");
        session.DisableOtpRequirement();
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        //
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var response = await GetHandler(testHelper).Handle(GetQuery("device", null), default);

        Assert.NotEmpty(response.Payload.Token);
    }

    [Fact]
    public async void WhenSessionIsTerminated_ThrowNotAllowedException()
    {
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();
        var session = UserSessionDbo.Create(seed.userId, "device", "0000");
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
        using var testHelper = new CoreTestHelper();
        var seed = testHelper.SeedDatabase();
        var session = UserSessionDbo.Create(seed.userId, "device", "0000");
        await testHelper.EssentialDb.UserSessions.AddAsync(session);
        await testHelper.EssentialDb.SaveChangesAsync(default);
        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId).Returns(session.SessionId);

        var query = GetQuery("device2", "0000");
        await Assert.ThrowsAsync<NotAllowedException>(() => GetHandler(testHelper).Handle(query, default));
    }
    #endregion

    private SessionUpdateCommand.UserTokenHandler GetHandler(CoreTestHelper coreTestHelper)
    {
        coreTestHelper.ConfigurationMock.Setup(x => x["Jwt:Key"]).Returns(new string('a', 30));
        return new SessionUpdateCommand.UserTokenHandler(
            coreTestHelper.EssentialDb,
            coreTestHelper.JwtService,
            coreTestHelper.MediatorMock.Object,
            coreTestHelper.ConfigurationMock.Object,
            coreTestHelper.CurrentUserInfoMock.Object,
            coreTestHelper.PasswordSecurity);
    }

    private SessionUpdateCommand GetQuery(string device, string otp)
    {
        return otp is null
            ? new SessionUpdateCommand(RequestDto.Create(device, 0, 0)) :
            new SessionUpdateCommand(RequestDto.Create(otp, device, 0, 0));
    }
}