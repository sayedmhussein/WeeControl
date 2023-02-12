using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WeeControl.ApiApp.Persistence.DbContexts;
using WeeControl.Core.Application.Contexts.User.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.Domain.Contexts.User;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Essential.Commands;

public class SessionCreateCommandTests
{
    private const string ValidUsername = "username";
    private const string ValidPassword = "password";
    
    #region Username and Password
    [Theory]
    [InlineData("", "", "")]
    [InlineData("device", "", "")]
    [InlineData("device", "username", "")]
    [InlineData("", "username", "")]
    [InlineData("", "", "password")]
    public async void WhenInvalidQueryParameters_ThrowBadRequestException(string device, string usernameOrEmail, string password)
    {
        using var testHelper = new TestHelper();
        
        var query = GetQuery(usernameOrEmail, password, device);

        await Assert.ThrowsAsync<BadRequestException>(() => GetHandler(testHelper).Handle(query, default));
    }
    
    [Theory]
    [InlineData("username@email.com", "password")]
    [InlineData("Username@Email.com", "password")]
    [InlineData(ValidUsername, ValidPassword)]
    [InlineData("Username", "password")]
    [InlineData("Username@email.com", "temporary")]
    [InlineData("Username", "temporary")]
    public async void WhenExistingUser_ReturnToken(string emailOrUsername, string password)
    {
        using var testHelper = new TestHelper();

        var response = await GetHandler(testHelper).Handle(GetQuery(emailOrUsername, password), default);

        Assert.NotEmpty(response.Payload.Token);
    }

    [Theory]
    [InlineData("username@email.com", "not password")]
    [InlineData("not email@email.com", "password")]
    [InlineData("username", "not password")]
    [InlineData("not username", "password")]
    [InlineData("Username@email.com", " not temporary")]
    [InlineData("not username", "temporary")]
    public async void WhenUserNotExist_ThrowNotFoundException(string emailOrUsername, string password)
    {
        using var testHelper = new TestHelper();

        var query = GetQuery(emailOrUsername, password);

        await Assert.ThrowsAsync<NotFoundException>(() => GetHandler(testHelper).Handle(query, default));
    }

    [Fact]
    public async void WhenSameUserLoginTwiceFromSameDevice_SessionShouldBeSame()
    {
        using var testHelper = new TestHelper();
        var query = GetQuery(ValidUsername, ValidPassword);
        
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
        using var testHelper = new TestHelper();
        var query = GetQuery(ValidUsername, ValidPassword);

        await GetHandler(testHelper).Handle(query, default);
        var count1 = await testHelper.EssentialDb.UserSessions.CountAsync();
        var session = await testHelper.EssentialDb.UserSessions.FirstOrDefaultAsync();
        Assert.NotNull(session);
        //
        session.TerminationTs = DateTime.Now;
        await testHelper.EssentialDb.SaveChangesAsync(default);

        await GetHandler(testHelper).Handle(query, default);
        var count2 = await testHelper.EssentialDb.UserSessions.CountAsync();

        Assert.Equal(count1 + 1, count2);
    }

    [Fact]
    public async void WhenSameUserLoginTwiceFromDifferentDevices_SessionShouldNotBeSame()
    {
        using var testHelper = new TestHelper();

        var query1 = GetQuery("username", "password", "device 1");
        await GetHandler(testHelper).Handle(query1, default);
        var session1 = await testHelper.EssentialDb.UserSessions.FirstOrDefaultAsync(x => x.DeviceId == "device 1");

        var query2 = GetQuery("username", "password", "device 2");
        await GetHandler(testHelper).Handle(query2, default);
        var session2 = await testHelper.EssentialDb.UserSessions.FirstOrDefaultAsync(x => x.DeviceId == "device 2");

        Assert.NotNull(session1);
        Assert.NotNull(session2);
        Assert.NotEqual(session1.SessionId, session2.SessionId);
    }
    #endregion

    private void SeedDb(TestHelper testHelper)
    {
        var person = PersonDbo.Create("FirstName", "LastName", "EGP", new DateOnly(1999, 12, 31));
        testHelper.EssentialDb.Person.Add(person);
        testHelper.EssentialDb.SaveChanges();

        var user = UserDbo.Create(person.PersonId, "username@email.com", ValidUsername, "0123456789", testHelper.PasswordSecurity.Hash(ValidPassword));
        user.SetTemporaryPassword(testHelper.PasswordSecurity.Hash("temporary")); 
        testHelper.EssentialDb.Users.Add(user);
        testHelper.EssentialDb.SaveChanges();
    }

    private SessionCreateCommand.SessionCreateHandler GetHandler(TestHelper testHelper)
    {
        testHelper.ConfigurationMock.Setup(x => x["Jwt:Key"]).Returns(new string('a', 30));
        
        if (testHelper.EssentialDb.Person.IsNullOrEmpty())
            SeedDb(testHelper);
        
        return new SessionCreateCommand.SessionCreateHandler(
            testHelper.EssentialDb,
            testHelper.JwtService,
            testHelper.ConfigurationMock.Object,
            testHelper.PasswordSecurity);
    }

    private SessionCreateCommand GetQuery(string emailOrUsername, string password, string device = "device")
    {
        return new SessionCreateCommand(RequestDto.Create(
            LoginRequestDto.Create(emailOrUsername, password), device, 0, 0));
    }
}