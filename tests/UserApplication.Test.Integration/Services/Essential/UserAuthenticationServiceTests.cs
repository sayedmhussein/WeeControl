using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.ApiApp.WebApi;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.AppInterfaces;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.Services.Essential;

public class UserAuthenticationServiceTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;
    
    public UserAuthenticationServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
    
    [Theory]
    [InlineData("username")]
    [InlineData("username@email.com")]
    public async void SuccessfulUserLoginAndGetTokenTest(string usernameOrEmail)
    {
        using var helper = new TestHelper<IAuthorizationService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient());
        
        Assert.True(await helper.Service.Login(usernameOrEmail,  "password" ));
        Assert.True(await helper.Service.UpdateToken("0000"));
        Assert.True(await helper.Service.IsAuthorized());
        Assert.True(await helper.Service.UpdateToken());
        Assert.True(await helper.Service.Logout());
    }
    
    [Theory]
    [InlineData("username", "passwordX")]
    [InlineData("usernameX", "password")]
    [InlineData("usernameX", "passwordX")]
    public async void InvalidUsernameOrPasswordTest(string username, string password)
    {
        using var helper = new TestHelper<IAuthorizationService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient());
        
        Assert.False(await helper.Service.Login(username, password));
        Assert.False(await helper.Service.UpdateToken("0000"));
        Assert.False(await helper.Service.IsAuthorized());
        Assert.False(await helper.Service.UpdateToken());
        Assert.False(await helper.Service.Logout());
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("00")]
    [InlineData("000")]
    [InlineData("1234")]
    public async void InvalidOrNoOtpTest(string otp)
    {
        using var helper = new TestHelper<IAuthorizationService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient());
        
        Assert.True(await helper.Service.Login("username", "password"));
        Assert.False(await helper.Service.UpdateToken(otp));
        //Assert.False(await helper.Service.IsAuthorized());
        //Assert.False(await helper.Service.UpdateToken());
        //Assert.False(await helper.Service.Logout());
    }
    
    #region BusinessLogic
    [Fact]
    public async void WhenUserIsLocked()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                user.Suspend("for testing");
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<IAuthorizationService>(httpClient);

        await helper.Service.Login("username", "password");
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
        
        helper.DeviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
    }
    #endregion
    
    [Fact]
    public async void Logout_WhenSuccess_AnyResponseFromServerAndDeleteToken()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<IAuthorizationService>(httpClient);
        await helper.Authorize("username", "password");

        await helper.Service.Logout();
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()));
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync(), Times.AtLeastOnce);
    }
}