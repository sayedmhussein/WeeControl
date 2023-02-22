using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.Test;
using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService.Contexts.User;

namespace WeeControl.Integration.Test.Contexts.Essential;

public class UserTests: IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public UserTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void TestHome()
    {
        using var hostTestHelper = new HostTestHelper();
        var service = hostTestHelper.GetService<IUserService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient());

        await factory.Authorize(hostTestHelper, CoreTestHelper.Username, CoreTestHelper.Password);

        Assert.True(await service.Refresh());
        Assert.NotEmpty(await service.GetFullName());
        Assert.NotEmpty(await service.GetNotifications());
    }
}