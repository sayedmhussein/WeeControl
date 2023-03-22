using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.SharedKernel.ExtensionMethods;
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

    public IEnumerable<HomeNotificationModel> Notifications { get; private set; } = new List<HomeNotificationModel>();
    public IEnumerable<HomeFeedModel> Feeds { get; private set; } = new List<HomeFeedModel>();
    public UserDataModel UserData { get; private set; } = new UserDataModel();

    public async Task<bool> PullData()
    {
        await server.RefreshToken();
        var response = await server
            .GetResponseMessage(HttpMethod.Get, new Version("1.0"), ApiRouting.Essentials.User.Route);
        if (response is null) return false;
        if (response.IsSuccessStatusCode)
        {
            var serverDto = await server.ReadFromContent<HomeResponseDto>(response.Content);
            if (serverDto is not null)
            {
                Notifications = serverDto.Notifications;
                Feeds = serverDto.Feeds;
                UserData.FullName = serverDto.FullName;
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
    
    public async Task ChangePassword(UserPasswordChangeRequestDto dto)
    {
        if (dto.IsValidEntityModel() == false)
        {
            await gui.DisplayAlert("invalid data");
            return;
        }

        var response = await server
            .GetResponseMessage(HttpMethod.Patch,
                new Version("1.0"), dto,
                ApiRouting.Essentials.User.Route,
                ApiRouting.Essentials.User.PasswordEndpoint);

        if (response is null) return;
        
        if (response.IsSuccessStatusCode)
        {
            await gui.DisplayAlert("Password was changed successfully");
            await gui.NavigateTo(ApplicationPages.Essential.HomePage);
            return;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            await gui.DisplayAlert("Old password isn't matching, please try again.");
            return;
        }

        await gui.DisplayAlert($"Unexpected Error {response.StatusCode}");
        throw new ArgumentOutOfRangeException();
    }

    public async Task SendFeedback(string message, IEnumerable<IBrowserFile> files)
    {
        await gui.DisplayQuickAlert("Thanks for your time.");
        var dto = new FeedbackDto();
        dto.FeedbackString = message;
        dto.Files = new List<IFormFile>();

        foreach (var f in files)
        {
            using var ms = new MemoryStream();
            f.OpenReadStream().CopyTo(ms);
            dto.Files.Add(new FormFile(ms, 0, ms.Length, "name", "fileName"));
        }
        
        
        await Task.Run(async () =>
        {
            var r = await server.GetResponseMessage(HttpMethod.Post, new Version("1.0"), dto, ApiRouting.Essentials.User.Route);
            await gui.DisplayAlert(await r.Content.ReadAsStringAsync());
            await Task.Delay(10000);
            await gui.DisplayQuickAlert("Your feedback was received successfully.", IGui.Severity.Success);
        });
    }
}