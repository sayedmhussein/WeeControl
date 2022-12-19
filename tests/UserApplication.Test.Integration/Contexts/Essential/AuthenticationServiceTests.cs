using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.ApiApp.WebApi;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
using WeeControl.Frontend.Service.UnitTest;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.Contexts.Essential;

public class AuthenticationServiceTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public AuthenticationServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
    
    [Theory]
    [InlineData("username", "password", true)]
    [InlineData("username@email.com", "password", true)]
    [InlineData("username", "passwordX", false)]
    [InlineData("usernameX", "password", false)]
    [InlineData("usernameX", "passwordX", false)]
    public async void LoginTest(string usernameOrEmail, string password, bool success)
    {
        using var h = new TestHelper(nameof(LoginTest));
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

        Assert.Equal(success, await service.Login(usernameOrEmail,  password ));
        Assert.Equal(success, await service.UpdateToken("0000"));
        Assert.Equal(success, await service.Logout());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("00")]
    [InlineData("000")]
    [InlineData("1234")]
    public async void UpdateTokenTests(string otp)
    {
        using var h = new TestHelper(nameof(UpdateTokenTests));
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
            x.NavigateToAsync(ApplicationPages.SplashPage, It.IsAny<bool>()), Times.Never);
        
        helper.DeviceMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
    }
    #endregion
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void LogoutTests(bool isLoggedIn)
    {
        using var h = new TestHelper(nameof(UpdateTokenTests));
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

        if (isLoggedIn)
        {
            Assert.True(await service.Login("username", "password"));
            Assert.True(await service.UpdateToken("0000"));
        }
        
        Assert.Equal(isLoggedIn, await service.Logout());
    }
}