using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService.Contexts.Essentials;

namespace WeeControl.Integration.Test.Contexts.Essential;

public class HomeServiceTests(CustomWebApplicationFactory<Startup> factory)
    : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    [Fact]
    public async void WhenRefreshIsCalled_HomeResponseDtoHasData()
    {
        using var hostTestHelper = new HostTestHelper(factory.CreateCustomClient());
        await hostTestHelper.Authenticate();
        var service = hostTestHelper.GetService<IHomeService>();

        await service.PullData();

        Assert.NotEmpty(service.UserData.FullName);
        Assert.NotEmpty(service.Notifications);
        Assert.NotEmpty(service.Feeds);
    }

    [Fact]
    public async void WhenNotificationIsViewed_ItGetViewedInDatabase()
    {
        using var helper = new HostTestHelper(factory.CreateCustomClient());
        await helper.Authenticate();
        var service = helper.GetService<IHomeService>();

        await service.PullData();
        var id = service.Notifications.First(x => x.ReadTs == null).NotificationId;

        await service.MarkNotificationAsViewed(id);
        await service.PullData();

        Assert.NotNull(service.Notifications.First(x => x.NotificationId == id).ReadTs);
    }
}