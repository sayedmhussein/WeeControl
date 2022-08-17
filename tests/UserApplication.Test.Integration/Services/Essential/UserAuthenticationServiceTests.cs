using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Interfaces;
using WeeControl.Frontend.ApplicationService;
using WeeControl.Frontend.ApplicationService.Contexts.Essential.Interfaces;
using WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.Services.Essential;

public class UserAuthenticationServiceTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    #region Preparation
    private readonly CustomWebApplicationFactory<Startup> factory;
    
    public UserAuthenticationServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
    #endregion

    #region Success
    [Theory]
    [InlineData("username")]
    [InlineData("username@email.com")]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode(string usernameOrEmail)
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
        
        using var helper = new TestHelper<IUserAuthorizationService>(httpClient);

        await helper.Service.Login(new LoginModel() {UsernameOrEmail = usernameOrEmail, Password = "password"});
        
        helper.DeviceMock.SecurityMock.Verify(x => x.
            UpdateTokenAsync(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.SplashPage, It.IsAny<bool>()), Times.Once);
    }
    #endregion

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
        
        using var helper = new TestHelper<IUserAuthorizationService>(httpClient);

        await helper.Service.Login(new LoginModel(){UsernameOrEmail = "username", Password = "password"});
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
        
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
        
        using var helper = new TestHelper<IUserAuthorizationService>(httpClient);
        await helper.Authorize("username", "password");

        await helper.Service.Logout();
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Essential.SplashPage, It.IsAny<bool>()));
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync(), Times.AtLeastOnce);
    }
}