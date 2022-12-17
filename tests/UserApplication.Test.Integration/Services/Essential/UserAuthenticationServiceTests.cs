using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.ApiApp.WebApi;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.Contexts.Home;
using WeeControl.Frontend.Service.UnitTest;
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
        using var h = new TestHelper(nameof(SuccessfulUserLoginAndGetTokenTest));
        var service = h.GetService<IAuthorizationService>(factory.WithWebHostBuilder(builder =>
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

        Assert.True(await service.Login(usernameOrEmail,  "password" ));
        Assert.True(await service.UpdateToken("0000"));
        Assert.True(await service.Logout());
    }
    
    [Theory]
    [InlineData("username", "passwordX")]
    [InlineData("usernameX", "password")]
    [InlineData("usernameX", "passwordX")]
    public async void InvalidUsernameOrPasswordTest(string username, string password)
    {
        using var h = new TestHelper(nameof(SuccessfulUserLoginAndGetTokenTest));
        var service = h.GetService<IAuthorizationService>(factory.WithWebHostBuilder(builder =>
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

        Assert.False(await service.Login(username, password));
        Assert.False(await service.UpdateToken("0000"));
        Assert.False(await service.Logout());
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
        using var h = new TestHelper(nameof(SuccessfulUserLoginAndGetTokenTest));
        var service = h.GetService<IAuthorizationService>(factory.WithWebHostBuilder(builder =>
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

        Assert.True(await service.Login("username", "password"));
        Assert.False(await service.UpdateToken(otp));
        Assert.False(await service.Logout());
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
        
        helper.DeviceMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
        
        helper.DeviceMock.Verify(x => 
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
        
        helper.DeviceMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()));
        
        // helper.DeviceMock.Verify(x => x.DeleteTokenAsync(), Times.AtLeastOnce);
    }
}