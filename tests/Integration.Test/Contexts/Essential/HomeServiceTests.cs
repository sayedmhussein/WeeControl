using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.Test;
using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService.Contexts.Essentials;

namespace WeeControl.Integration.Test.Contexts.Essential;

public class HomeServiceTests: IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public HomeServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void TestHome()
    {
        using var hostTestHelper = new HostTestHelper();
        var service = hostTestHelper.GetService<IHomeService>(factory.WithWebHostBuilder(builder =>
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

    [Fact]
    public async void WhenNotificationIsViewed_ItGetViewedInDatabase()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IHomeService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient());
        
        await factory.Authorize(helper, CoreTestHelper.Username, CoreTestHelper.Password);

        var unreadNotifications1 = (await service.GetNotifications())
            .Where(x => x.ReadTs == null);
        await service.MarkNotificationAsViewed(unreadNotifications1.First().NotificationId);
        await service.Refresh();
        var unreadNotifications2 = (await service.GetNotifications())
            .Where(x => x.ReadTs == null);
        
        Assert.Equal(unreadNotifications1.Count(), unreadNotifications2.Count() + 1);
    }
}