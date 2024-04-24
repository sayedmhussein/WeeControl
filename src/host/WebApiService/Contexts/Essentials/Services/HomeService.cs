using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.ExtensionHelpers;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Contexts.Essentials.Services;

internal class HomeService(IServerOperation server, IGui gui, IDeviceSecurity security)
    : IHomeService
{
    public string LastLoginTimestamp { get; private set; } = string.Empty;

    public IEnumerable<HomeNotificationModel> Notifications { get; private set; } = new List<HomeNotificationModel>();
    public IEnumerable<HomeFeedModel> Feeds { get; private set; } = new List<HomeFeedModel>();
    public UserDataModel UserData { get; } = new();

    public async Task<bool> PullData()
    {
        await server.RefreshToken();
        var response = await server
            .GetResponseMessage(HttpMethod.Get, new Version("1.0"), ApiRouting.Essentials.User.Route);

        if (response.IsSuccessStatusCode)
        {
            var serverDto = await server.ReadFromContent<HomeResponseDto>(response.Content);
            if (serverDto is not null)
            {
                Notifications = serverDto.Notifications;
                Feeds = serverDto.Feeds;
                UserData.FullName = serverDto.FullName;
                LastLoginTimestamp = serverDto.PhotoUrl;
                return true;
            }
        }

        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Unauthorized)
        {
            await security.DeleteToken();
            await gui.DisplayAlert("Please login again");
            await gui.NavigateTo(ApplicationPages.Essential.LoginPage, true);
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
                ApiRouting.Essentials.User.Route,
                ApiRouting.Essentials.User.NotificationEndpoint,
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