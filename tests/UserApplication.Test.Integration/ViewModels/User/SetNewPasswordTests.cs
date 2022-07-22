using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Interfaces;
using WeeControl.User.UserApplication.ViewModels.Essential.User;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.User;

public class SetNewPasswordTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public SetNewPasswordTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void WhenSuccess()
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
        
        using var helper = new TestHelper<SetNewPasswordViewModel>(httpClient);
        await helper.Authorize("username", "password");

        helper.ViewModel.OldPassword = "password";
        helper.ViewModel.NewPassword = "someNewPassword";
        helper.ViewModel.ConfirmNewPassword = "someNewPassword";

        await helper.ViewModel.ChangeMyPassword();
            
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async void WhenUnauthorized()
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
        
        using var helper = new TestHelper<SetNewPasswordViewModel>(httpClient);

        helper.ViewModel.OldPassword = "password";
        helper.ViewModel.NewPassword = "someNewPassword";
        helper.ViewModel.ConfirmNewPassword = "someNewPassword";

        await helper.ViewModel.ChangeMyPassword();
            
        helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenBusinessNotAllow_InvalidPassword()
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
        
        using var helper = new TestHelper<SetNewPasswordViewModel>(httpClient);
        await helper.Authorize("username", "password");

        helper.ViewModel.OldPassword = "invalid password";
        helper.ViewModel.NewPassword = "someNewPassword";
        helper.ViewModel.ConfirmNewPassword = "someNewPassword";

        await helper.ViewModel.ChangeMyPassword();
            
        helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }

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
                user.Suspend("This is for testing only");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<SetNewPasswordViewModel>(httpClient);
        await helper.Authorize("username", "password");

        helper.ViewModel.OldPassword = "password";
        helper.ViewModel.NewPassword = "someNewPassword";
        helper.ViewModel.ConfirmNewPassword = "someNewPassword";

        await helper.ViewModel.ChangeMyPassword();
            
        helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
    }
}