using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Contexts.Essentials.Services;

internal class HomeService : IHomeService
{
    private readonly IGui gui;
    private readonly IDeviceSecurity security;
    private readonly IServerOperation server;

    public HomeService(IServerOperation server, IGui gui, IDeviceSecurity security)
    {
        this.server = server;
        this.gui = gui;
        this.security = security;
    }

    public string LastLoginTimestamp { get; private set; } = string.Empty;

    public IEnumerable<HomeNotificationModel> Notifications { get; private set; } = new List<HomeNotificationModel>();
    public IEnumerable<HomeFeedModel> Feeds { get; private set; } = new List<HomeFeedModel>();
    public string Fullname { get; private set; } = string.Empty;

    public async Task<bool> Refresh()
    {
        await server.RefreshToken();
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
            await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage, true);
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
                ControllerApi.Essentials.User.NotificationEndpoint,
                new[] {"id", id.ToString()});

        return response;
    }

    public async Task SendFeedback(string message, IEnumerable<IBrowserFile> files)
    {
        await gui.DisplayQuickAlert("Thanks for your time.");
        await Task.Run(async () =>
        {
            await Task.Delay(10000);
            await gui.DisplayQuickAlert("Your feedback was received successfully.", IGui.Severity.Success);
        });
    }
}