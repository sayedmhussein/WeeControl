using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.WebApi;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.Service.UnitTest;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.Contexts;

public class ExampleTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public ExampleTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void TestAuthorizeInCustomerWebApplication()
    {
        using var helper = new TestHelper(nameof(TestAuthorizeInCustomerWebApplication));
        var service = helper.GetService<IServiceData>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = factory.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient());

        await factory.Authorize(helper, "username", "password");

        Assert.True(await service.IsAuthenticated());
    }
}