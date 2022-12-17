using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.Contexts.Home;
using WeeControl.Frontend.AppService.Internals.Interfaces;

namespace WeeControl.Frontend.AppService.Contexts.Home;

internal class HomeService : IHomeService
{
    private readonly IDeviceData device;
    private readonly IServerOperation server;
    private readonly IDatabaseService db;

    public HomeService(IDeviceData device, IServerOperation server, IDatabaseService db)
    {
        this.device = device;
        this.server = server;
        this.db = db;
    }
    
    public async Task<bool> Sync()
    {
        var response = await server.Send(new HttpRequestMessage
        {
            RequestUri = new Uri(server.GetFullAddress(ApiRouting.HomeRoute)),
            Version = new Version("1.0"),
            Method = HttpMethod.Get
        });

        if (response.IsSuccessStatusCode)
        {
            var dto = await server.ReadFromContent<HomeResponseDto>(response.Content);
            
        }

        return false;
    }

    public Task<string> GetGreetingMessage()
    {
        return Task.FromResult("Hello");
    }

    public Task<IEnumerable<HomeFeedModel>> GetHomeFeeds()
    {
        return db.ReadFromTable<HomeFeedModel>();
    }

    public Task<IEnumerable<HomeNotificationModel>> GetHomeNotifications()
    {
        return db.ReadFromTable<HomeNotificationModel>();
    }
}