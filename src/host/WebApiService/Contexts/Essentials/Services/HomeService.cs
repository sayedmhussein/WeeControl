using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Contexts.Essentials.Services;

internal class HomeService : IHomeService
{
    private readonly IServerOperation server;
    private readonly IGui gui;
    private readonly IDeviceSecurity security;

    public HomeService(IServerOperation server, IGui gui, IDeviceSecurity security)
    {
        this.server = server;
        this.gui = gui;
        this.security = security;
    }

    public IEnumerable<HomeNotificationModel> Notifications { get; private set; } = new List<HomeNotificationModel>();
    public IEnumerable<HomeFeedModel> Feeds { get; private set; } = new List<HomeFeedModel>();
    public string Fullname { get; private set; } = string.Empty;
    public string LastLoginTimestamp { get; private set; } = string.Empty;

    public async Task<bool> Refresh()
    {
        var response = await server
            .GetResponseMessage(HttpMethod.Get, new Version("1.0"), ControllerApi.Essentials.User.Route);

        if (response.IsSuccessStatusCode)
        {
            var serverDto = await server.ReadFromContent<HomeResponseDto>(response.Content);
            if (serverDto is not null)
            {
                Notifications = serverDto.Notifications;
                Feeds = serverDto.Feeds;
                Fullname = serverDto.FullName;
                LastLoginTimestamp = serverDto.PhotoUrl;
                return true;
            }
        }

        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Unauthorized)
        {
            await security.DeleteToken();
            await gui.DisplayAlert("Please login again");
            await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage, forceLoad: true);
            return false;
        }
        
        await gui.DisplayAlert($"Unexpected error occured when communicating with server! {response.StatusCode}");
        return false;
    }

    public Task MarkNotificationAsViewed(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        var response = server
            .GetResponseMessage(HttpMethod.Delete, 
                new Version("1.0"), 
                ControllerApi.Essentials.User.Route,
                endpoint:ControllerApi.Essentials.User.NotificationEndpoint,
                query: new []{"id", id.ToString()});

        return response;
    }
}