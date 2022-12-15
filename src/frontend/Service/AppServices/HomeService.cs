using System.Net.Http.Json;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.DataTransferObjects.User;
using WeeControl.Frontend.AppService.AppInterfaces;
using WeeControl.Frontend.AppService.AppModels;
using WeeControl.Frontend.AppService.Interfaces;

namespace WeeControl.Frontend.AppService.AppServices;

internal class HomeService : IHomeService
{
    private readonly IDevice device;
    private readonly IServerOperation server;
    private readonly IDatabaseService db;

    public HomeService(IDevice device, IServerOperation server, IDatabaseService db)
    {
        this.device = device;
        this.server = server;
        this.db = db;
    }
    
    public async Task<bool> Sync()
    {
        var response = await server.Send(new HttpRequestMessage
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Route)),
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