using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Interfaces;
using WeeControl.Frontend.ApplicationService;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;

public class LogoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public LogoutTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void WhenSuccess_AnyResponseFromServerAndDeleteToken()
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
        
        using var helper = new TestHelper<AuthorizationViewModel>(httpClient);
        await helper.Authorize("username", "password");

        await helper.ViewModel.Logout();
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Customer.Authentication.LoginPage, It.IsAny<bool>()), Times.Once);
        
        helper.DeviceMock.SecurityMock.Verify(x => x.DeleteTokenAsync(), Times.AtLeastOnce);
    }
}