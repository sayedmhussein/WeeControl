using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Interfaces;
using WeeControl.User.UserApplication.ViewModels.Essential;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;

public class LoginTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    #region Preparation
    private readonly CustomWebApplicationFactory<Startup> factory;
    
    public LoginTests(CustomWebApplicationFactory<Startup> factory)
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
        
        using var helper = new TestHelper<LoginViewModel>(httpClient);
        helper.ViewModel.UsernameOrEmail = usernameOrEmail;
        helper.ViewModel.Password = "password";

        await helper.ViewModel.LoginAsync();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.
            UpdateTokenAsync(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Once);
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
        
        using var helper = new TestHelper<LoginViewModel>(httpClient);
        helper.ViewModel.UsernameOrEmail = "username";
        helper.ViewModel.Password = "password";

        await helper.ViewModel.LoginAsync();
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
        
        helper.DeviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
    }
    #endregion
}