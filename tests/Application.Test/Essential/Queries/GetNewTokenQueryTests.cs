using System;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Essential.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Application.Test.Essential.Queries;

public class GetNewTokenQueryTests : TestHelper
{
    private readonly GetNewTokenQuery.GetNewTokenHandler handler;
    
    public GetNewTokenQueryTests()
    {
        ConfigurationMock.Setup(x => x["Jwt:Key"]).Returns(new string('a', 30));

        handler = new GetNewTokenQuery.GetNewTokenHandler(
            EssentialDb, 
            JwtService, 
            MediatorMock.Object, 
            ConfigurationMock.Object, 
            CurrentUserInfoMock.Object, 
            PasswordSecurity);
    }

    #region Username and Password
    [Theory]
    [InlineData("email@email.com", "password")]
    [InlineData("Email@Email.com", "password")]
    [InlineData("username", "password")]
    [InlineData("Username", "password")]
    [InlineData("Email@email.com", "temporary")]
    [InlineData("Username", "temporary")]
    public async void WhenExistingUser_ReturnToken(string emailOrUsername, string password)
    {
        var user = UserDbo.Create(
            "email@email.com", 
            "username", 
            PasswordSecurity.Hash("password"), 
            "TRR");
        user.SetTemporaryPassword(PasswordSecurity.Hash("temporary"));
        await EssentialDb.Users.AddAsync(user);
        await EssentialDb.SaveChangesAsync(default);

        var query = new GetNewTokenQuery(RequestDto.Create(
            LoginDtoV1.Create(emailOrUsername, password), "device", 0, 0));

        var response = await handler.Handle(query, default);
        
        Assert.NotEmpty(response.Payload.Token);
        Assert.NotEmpty(response.Payload.FullName);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("device", "", "")]
    [InlineData("device", "username", "")]
    [InlineData("", "username", "")]
    [InlineData("", "", "password")]
    public async void WhenInvalidQueryParameters_ThrowBadRequestException(string device, string usernameOrEmail, string password)
    {
        var query = new GetNewTokenQuery(RequestDto.Create(
            LoginDtoV1.Create(usernameOrEmail, password), device, 0, 0));
        
        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(query, default));
    }

    [Theory]
    [InlineData("email@email.com", "not password")]
    [InlineData("not email@email.com", "password")]
    [InlineData("username", "not password")]
    [InlineData("not username", "password")]
    [InlineData("Email@email.com", " not temporary")]
    [InlineData("not username", "temporary")]
    public async void WhenUserNotExist_ThrowNotFoundException(string emailOrUsername, string password)
    {
        var user = UserDbo.Create(
            "email@email.com", 
            "username", 
            PasswordSecurity.Hash("password"), 
            "TRR");
        user.SetTemporaryPassword(PasswordSecurity.Hash("temporary"));
        await EssentialDb.Users.AddAsync(user);
        await EssentialDb.SaveChangesAsync(default);
        
        var query = new GetNewTokenQuery(RequestDto.Create(
            LoginDtoV1.Create(emailOrUsername, password),  "device", 0, 0));
        
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, default));
    }
    
    [Fact]
    public async void WhenSameUserLoginTwiceFromSameDevice_SessionShouldBeSame()
    {
        var user = UserDbo.Create(
            "email@email.com", 
            "username", 
            PasswordSecurity.Hash("password"), 
            "TRR");
        await EssentialDb.Users.AddAsync(user);
        await EssentialDb.SaveChangesAsync(default);

        var query = new GetNewTokenQuery(RequestDto.Create(
     
            LoginDtoV1.Create("username", "password"),       "device",  0, 0));

        await handler.Handle(query, default);
        var count1 = await EssentialDb.Sessions.CountAsync();
        
        await handler.Handle(query, default);
        var count2 = await EssentialDb.Sessions.CountAsync();
        
        Assert.Equal(count1, count2);
    }

    [Fact]
    public async void WhenSameUserLoginTwiceFromSameDeviceButAfterLoggedOutFirstTime_SessionShouldNotBeSame()
    {
        var user = UserDbo.Create(
            "email@email.com", 
            "username", 
            PasswordSecurity.Hash("password"), 
            "TRR");
        await EssentialDb.Users.AddAsync(user);
        await EssentialDb.SaveChangesAsync(default);

        var query = new GetNewTokenQuery(RequestDto.Create(
            LoginDtoV1.Create("username", "password"),"device",  0, 0));

        await handler.Handle(query, default);
        var count1 = await EssentialDb.Sessions.CountAsync();
        var session = await EssentialDb.Sessions.FirstOrDefaultAsync(x => x.UserId == user.UserId);
        
        Assert.NotNull(session);
        session.TerminationTs = DateTime.Now;
        await EssentialDb.SaveChangesAsync(default);
        
        await handler.Handle(query, default);
        var count2 = await EssentialDb.Sessions.CountAsync();
        
        Assert.Equal(count1 + 1, count2);
    }
    
    [Fact]
    public async void WhenSameUserLoginTwiceFromDifferentDevices_SessionShouldNotBeSame()
    {
        var user = UserDbo.Create(
            "email@email.com", 
            "username", 
            PasswordSecurity.Hash("password"), 
            "TRR");
        await EssentialDb.Users.AddAsync(user);
        await EssentialDb.SaveChangesAsync(default);

        var query1 = new GetNewTokenQuery(RequestDto.Create(
            LoginDtoV1.Create("username", "password"),"device 1",  0, 0));
        await handler.Handle(query1, default);
        var session1 = await EssentialDb.Sessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.DeviceId == "device 1");

        var query2 = new GetNewTokenQuery(RequestDto.Create(
            LoginDtoV1.Create("username", "password"),"device 2",  0, 0));
        await handler.Handle(query2, default);
        var session2 = await EssentialDb.Sessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.DeviceId == "device 2");
        
        Assert.NotNull(session1);
        Assert.NotNull(session2);
        Assert.NotEqual(session1.SessionId, session2.SessionId);
    }
    #endregion

    #region Using SessionId
    [Fact]
    public async void WhenSessionIsActive_ReturnToken()
    {
        var user = UserDbo.Create(
            "email@email.com", 
            "username", 
            PasswordSecurity.Hash("password"), 
            "TRR");
        user.SetTemporaryPassword(PasswordSecurity.Hash("temporary"));
        await EssentialDb.Users.AddAsync(user);
        await EssentialDb.SaveChangesAsync(default);

        await handler.Handle(new GetNewTokenQuery(RequestDto.Create(
                LoginDtoV1.Create("username", "password"), "device", 0, 0)), 
            default);
        
        var session = await EssentialDb.Sessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.DeviceId == "device");
        Assert.NotNull(session);
        CurrentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

        var response = await handler.Handle(new GetNewTokenQuery(RequestDto.Create("device", 0, 0)), default);

        Assert.NotEmpty(response.Payload.Token);
        Assert.NotEmpty(response.Payload.FullName);
    }
    
    [Fact]
    public async void WhenSessionIsTerminated_ThrowNotAllowedException()
    {
        var user = UserDbo.Create(
            "email@email.com", 
            "username", 
            PasswordSecurity.Hash("password"), 
            "TRR");
        user.SetTemporaryPassword(PasswordSecurity.Hash("temporary"));
        await EssentialDb.Users.AddAsync(user);
        await EssentialDb.SaveChangesAsync(default);

        await handler.Handle(new GetNewTokenQuery(RequestDto.Create(
                LoginDtoV1.Create("username", "password"), "device", 0, 0)), 
            default);
        
        var session = await EssentialDb.Sessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.DeviceId == "device");
        Assert.NotNull(session);
        session.TerminationTs = DateTime.Now;
        await EssentialDb.SaveChangesAsync(default);
        CurrentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

        var query = RequestDto.Create("device", 0, 0);
        await Assert.ThrowsAsync<NotAllowedException>(() => handler.Handle(new GetNewTokenQuery(query), default));
    }
    
    [Fact]
    public async void WhenSessionIsActiveButFromDifferentDevice_ThrowNotAllowedException()
    {
        var user = UserDbo.Create(
            "email@email.com", 
            "username", 
            PasswordSecurity.Hash("password"), 
            "TRR");
        user.SetTemporaryPassword(PasswordSecurity.Hash("temporary"));
        await EssentialDb.Users.AddAsync(user);
        await EssentialDb.SaveChangesAsync(default);

        await handler.Handle(new GetNewTokenQuery(RequestDto.Create(
                LoginDtoV1.Create("username", "password"),"device",  0, 0)), 
            default);
        
        var session = await EssentialDb.Sessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.DeviceId == "device");
        Assert.NotNull(session);
        CurrentUserInfoMock.Setup(x => x.GetSessionId()).Returns(session.SessionId);

        var query = RequestDto.Create("different device", 0, 0);
        await Assert.ThrowsAsync<NotAllowedException>(() => handler.Handle(new GetNewTokenQuery(query), default));
    }
    #endregion
}