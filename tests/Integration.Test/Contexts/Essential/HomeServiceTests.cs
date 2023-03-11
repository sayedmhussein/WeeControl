using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService.Contexts.Essentials;

namespace WeeControl.Integration.Test.Contexts.Essential;

public class HomeServiceTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public HomeServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void WhenRefreshIsCalled_HomeResponseDtoHasData()
    {
        using var hostTestHelper = new HostTestHelper(factory.CreateCustomClient());
        await hostTestHelper.Authenticate();
        var service = hostTestHelper.GetService<IHomeService>();

        await service.Refresh();

        Assert.NotEmpty(service.Fullname);
        Assert.NotEmpty(service.Notifications);
        Assert.NotEmpty(service.Feeds);
    }

    [Fact]
    public async void WhenNotificationIsViewed_ItGetViewedInDatabase()
    {
        using var helper = new HostTestHelper(factory.CreateCustomClient());
        await helper.Authenticate();
        var service = helper.GetService<IHomeService>();

        await service.Refresh();
        var id = service.Notifications.First(x => x.ReadTs == null).NotificationId;

        await service.MarkNotificationAsViewed(id);
        await service.Refresh();

        Assert.NotNull(service.Notifications.First(x => x.NotificationId == id).ReadTs);
    }
}