using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.User.UserApplication.ViewModels.Authentication;
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
    public async void WhenSuccess()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<LogoutViewModel>(httpClient);
        await helper.Authorize("", "");

        await helper.ViewModel.LogoutAsync();
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Once);
    }
}