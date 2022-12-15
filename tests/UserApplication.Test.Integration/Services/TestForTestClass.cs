using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.ApiApp.WebApi;
using WeeControl.Frontend.AppService.AppInterfaces;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.Services;

public class TestForTestClass : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public TestForTestClass(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
    
    [Fact]
    public async void TestLoginWhenSuccess()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                db.Users.Add(TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password"));
                db.SaveChanges();
            });
        }).CreateClient();

        using var helper = new TestHelper<IAuthorizationService>(httpClient);
        await helper.Authorize("username", "password", "device");

        var token = await helper.Device.Security.GetTokenAsync();
        
        Assert.NotEmpty(token);
    }
    
    [Fact]
    public async void TestLoginWhenFailure()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                db.Users.Add(TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password"));
                db.SaveChanges();
            });
        }).CreateClient();

        using var helper = new TestHelper<IAuthorizationService>(httpClient);
        await helper.Authorize("username1", "password");

        var token = await helper.Device.Security.GetTokenAsync();
        
        Assert.Empty(token);
    }
}