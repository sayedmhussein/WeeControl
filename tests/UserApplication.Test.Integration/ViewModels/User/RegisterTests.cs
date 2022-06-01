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

public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public RegisterTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
    
    [Fact]
    public async void WhenSuccess()
    {
        using var helper = new TestHelper<RegisterViewModel>(factory.CreateClient());
        helper.ViewModel.Email = "email@email.com";
        helper.ViewModel.Username = "someUsername";
        helper.ViewModel.Password = "somePassword";
        
        await helper.ViewModel.RegisterAsync();

        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Once);
    }
    
    [Theory]
    [InlineData("email@email.com", "username")]
    [InlineData("email@email.com1", "username")]
    [InlineData("email@email.com", "username1")]
    public async void WhenBusinessNotAllow_ExistingUser(string email, string username)
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
                db.Users.Add(user);
                user.Suspend("for testing");
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<RegisterViewModel>(httpClient);
        
        helper.ViewModel.Email = email;
        helper.ViewModel.Username = username;
        helper.ViewModel.Password = "somePassword";
        
        await helper.ViewModel.RegisterAsync();
            
        helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);;
    }
}