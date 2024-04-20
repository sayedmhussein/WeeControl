using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Contexts.Essentials.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.DtoParent;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Commands;

public class SessionCreateCommandTests
{
    private static SessionCreateCommand.SessionCreateHandler GetHandler(CoreTestHelper coreTestHelper)
    {
        coreTestHelper.ConfigurationMock.Setup(x => x["Jwt:Key"]).Returns(new string('a', 30));

        return new SessionCreateCommand.SessionCreateHandler(
            coreTestHelper.EssentialDb,
            coreTestHelper.JwtService,
            coreTestHelper.ConfigurationMock.Object,
            coreTestHelper.PasswordSecurity);
    }

    private static SessionCreateCommand GetQuery(string emailOrUsername, string password, string device = "device")
    {
        return new SessionCreateCommand(RequestDto.Create(
            LoginRequestDto.Create(emailOrUsername, password), device, 0, 0));
    }

    #region Username and Password

    [Theory]
    [InlineData("", "", "")]
    [InlineData("device", "", "")]
    [InlineData("device", "username", "")]
    [InlineData("", "username", "")]
    [InlineData("", "", "password")]
    public async void WhenInvalidQueryParameters_ThrowBadRequestException(string device, string usernameOrEmail,
        string password)
    {
        using var testHelper = new CoreTestHelper();

        var query = GetQuery(usernameOrEmail, password, device);

        await Assert.ThrowsAsync<BadRequestException>(() => GetHandler(testHelper).Handle(query, default));
    }

    [Theory]
    [InlineData("username@Email.com", "password")]
    [InlineData("Username@Email.com", "password")]
    [InlineData(CoreTestHelper.Email, CoreTestHelper.Password)]
    [InlineData(CoreTestHelper.Username, "password")]
    [InlineData("Username@email.com", "temporary")]
    [InlineData("Username", "temporary")]
    public async void WhenExistingUser_ReturnToken(string emailOrUsername, string password)
    {
        using var testHelper = new CoreTestHelper();
        testHelper.SeedDatabase();
        var user = await testHelper.EssentialDb.Users.FirstAsync(x => x.Username == CoreTestHelper.Username);
        user.SetTemporaryPassword(testHelper.PasswordSecurity.Hash("temporary"));
        await testHelper.EssentialDb.SaveChangesAsync(default);

        var response = await GetHandler(testHelper).Handle(GetQuery(emailOrUsername, password), default);

        Assert.NotEmpty(response.Payload.Token);
        Assert.NotEmpty(testHelper.EssentialDb.UserSessions);
    }

    [Theory]
    [InlineData("username@email.com", "not password")]
    [InlineData("not_email@email.com", "password")]
    [InlineData("username", "not password")]
    [InlineData("not_username", "password")]
    [InlineData("Username@email.com", " not temporary")]
    [InlineData("not_username", "temporary")]
    public async void WhenUserNotExist_ThrowNotFoundException(string emailOrUsername, string password)
    {
        using var testHelper = new CoreTestHelper();
        testHelper.SeedDatabase();

        var query = GetQuery(emailOrUsername, password);

        await Assert.ThrowsAsync<NotFoundException>(() => GetHandler(testHelper).Handle(query, default));
    }

    [Fact]
    public async void WhenSameUserLoginTwiceFromSameDevice_SessionShouldBeSame()
    {
        using var testHelper = new CoreTestHelper();
        var entity = testHelper.SeedDatabase();
        var query = GetQuery(CoreTestHelper.Username, CoreTestHelper.Password);

        await GetHandler(testHelper).Handle(query, default);
        var count1 = await testHelper.EssentialDb.UserSessions.CountAsync();
        //
        await GetHandler(testHelper).Handle(query, default);
        var count2 = await testHelper.EssentialDb.UserSessions.CountAsync();

        Assert.Equal(count1, count2);
    }

    [Fact]
    public async void WhenSameUserLoginTwiceFromSameDeviceButAfterLoggedOutFirstTime_SessionShouldNotBeSame()
    {
        const string deviceName = "DeviceName";
        //
        using var testHelper = new CoreTestHelper();
        var entity = testHelper.SeedDatabase();
        var query = GetQuery(CoreTestHelper.Username, CoreTestHelper.Password, deviceName);

        await GetHandler(testHelper).Handle(query, default);
        var count1 = await testHelper.EssentialDb.UserSessions.Where(x => x.DeviceId == deviceName).CountAsync();
        var session = await testHelper.EssentialDb.UserSessions.Where(x => x.DeviceId == deviceName)
            .FirstOrDefaultAsync();
        Assert.NotNull(session);
        //
        session.TerminationTs = DateTime.Now;
        await testHelper.EssentialDb.SaveChangesAsync(default);

        await GetHandler(testHelper).Handle(query, default);
        var count2 = await testHelper.EssentialDb.UserSessions.Where(x => x.DeviceId == deviceName).CountAsync();

        Assert.Equal(count1 + 1, count2);
    }

    [Fact]
    public async void WhenSameUserLoginTwiceFromDifferentDevices_SessionShouldNotBeSame()
    {
        using var testHelper = new CoreTestHelper();
        var entity = testHelper.SeedDatabase();

        var query1 = GetQuery(CoreTestHelper.Username, CoreTestHelper.Password, "device 1");
        await GetHandler(testHelper).Handle(query1, default);
        var session1 = await testHelper.EssentialDb.UserSessions.FirstOrDefaultAsync(x => x.DeviceId == "device 1");

        var query2 = GetQuery(CoreTestHelper.Username, CoreTestHelper.Password, "device 2");
        await GetHandler(testHelper).Handle(query2, default);
        var session2 = await testHelper.EssentialDb.UserSessions.FirstOrDefaultAsync(x => x.DeviceId == "device 2");

        Assert.NotNull(session1);
        Assert.NotNull(session2);
        Assert.NotEqual(session1.SessionId, session2.SessionId);
    }

    #endregion
}