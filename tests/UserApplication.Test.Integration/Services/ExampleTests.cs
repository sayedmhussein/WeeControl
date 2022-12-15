using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.ApiApp.WebApi;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.AppInterfaces;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.Services;

public class ExampleTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;
    
    public ExampleTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void Test1_()
    {
        using var helper = new TestHelper<IAuthorizationService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                //Prepare database as required
                db.SaveChanges();
            });
        }).CreateClient());
        
        await helper.Service.Login("username", "password");
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
    }
}