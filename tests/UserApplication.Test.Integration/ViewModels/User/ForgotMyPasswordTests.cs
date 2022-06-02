using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.ViewModels.User;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.User;

public class ForgotMyPasswordTests: IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public ForgotMyPasswordTests(CustomWebApplicationFactory<Startup> factory)
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
                db.Users.Add(UserDbo.Create(
                    "email@email.com", 
                    "username", 
                    TestHelper<object>.GetEncryptedPassword("password")));
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<ForgotMyPasswordViewModel>(httpClient);
        
        helper.ViewModel.Email = "email@email.com";
        helper.ViewModel.Username = "username";
        
        await helper.ViewModel.RequestPasswordReset();
            
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Once);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("email@email.com", "")]
    [InlineData("", "username")]
    public async void WhenInvalidEmailAndUsernameMatchingOrNotExist(string email, string username)
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                db.Users.Add(UserDbo.Create(
                    "email@email.com", 
                    "username", 
                    TestHelper<object>.GetEncryptedPassword("password")));
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<ForgotMyPasswordViewModel>(httpClient);
        
        helper.ViewModel.Email = email;
        helper.ViewModel.Username = username;
        
        await helper.ViewModel.RequestPasswordReset();
            
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async void WhenBusinessNotAllow_IsLockedUser()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = UserDbo.Create(
                    "email@email.com",
                    "username",
                    TestHelper<object>.GetEncryptedPassword("password"));
                user.Suspend("for testing");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<ForgotMyPasswordViewModel>(httpClient);
        
        helper.ViewModel.Email = "email@email.com";
        helper.ViewModel.Username = "username";
        
        await helper.ViewModel.RequestPasswordReset();
            
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
}