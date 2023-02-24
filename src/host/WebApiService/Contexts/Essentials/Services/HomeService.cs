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
    private HomeResponseDto? dto;

    public HomeService(IServerOperation server, IGui gui)
    {
        this.server = server;
        this.gui = gui;
    }
    
    public async Task<bool> Refresh()
    {
        var response = await server
            .GetResponseMessage(HttpMethod.Get, new Version("1.0"), ControllerApi.Essentials.User.Route);

        if (response.IsSuccessStatusCode)
        {
            var serverDto = await server.ReadFromContent<HomeResponseDto>(response.Content);
            if (serverDto is not null)
            {
                dto = serverDto;
                return true;
            }
        }
        else if (response.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized or HttpStatusCode.NotFound)
        {
            if (await server.RefreshToken())
            {
                await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage, forceLoad:true);
                return false;
            }
        }

        await gui.DisplayAlert($"Unexpected error occured when communicating with server! {response.StatusCode}");
        return false;
    }

    public async Task<IEnumerable<HomeNotificationModel>> GetNotifications()
    {
        if (dto == null)
            await Refresh();

        return dto?.Notifications ?? new List<HomeNotificationModel>();
    }

    public async Task<IEnumerable<HomeFeedModel>> GetFeeds()
    {
        if (dto == null)
            await Refresh();

        return dto?.Feeds ?? new List<HomeFeedModel>();
    }

    public async Task<string> GetFullName()
    {
        if (dto == null)
            await Refresh();

        return dto?.FullName ?? string.Empty;
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