using System.Net.Http.Json;
using WeeControl.Frontend.ApplicationService.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly IServerOperation server;

    public string GreetingMessage { get; private set; } = "Hello";
    public string NameOfUser { get; private set; } = string.Empty;
    public List<MenuItemModel> MenuItems { get; }
    public List<HomeFeedModel> FeedsList { get; }
    public List<HomeNotificationModel> NotificationsList { get; }

    public HomeViewModel(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;
        MenuItems = new List<MenuItemModel>();
        FeedsList = new List<HomeFeedModel>();
        NotificationsList = new List<HomeNotificationModel>();
    }

    public async Task Init()
    {
        if (await server.IsTokenValid() && await device.Security.IsAuthenticatedAsync())
        {
            NameOfUser = await device.Storage.GetAsync(nameof(AuthenticationResponseDto.FullName));
            await device.Navigation.NavigateToAsync(Pages.Shared.IndexPage);
            await SetupMenuAsync();
            return;
        }

        await device.Navigation.NavigateToAsync(Pages.Essential.Authentication.LoginPage);
    }

    public Task ChangeMyPasswordAsync()
    {
        return device.Navigation.NavigateToAsync(Pages.Essential.User.SetNewPasswordPage);
    }

    private async Task SetupMenuAsync()
    {
        var list = new List<MenuItemModel>();

        foreach (var claim in await device.Security.GetClaimsAsync())
        {
            if (claim.Type is ClaimsValues.ClaimTypes.Session or ClaimsValues.ClaimTypes.Territory)
            {
                continue;
            }

            if (ClaimsValues.GetClaimTypes().ContainsValue(claim.Type))
            {
                var name = ClaimsValues.GetClaimTypes().First(x => x.Value == claim.Type);
                list.Add(MenuItemModel.Create(name.Key));
            }
        }

        MenuItems.Clear();
        MenuItems.AddRange(list.DistinctBy(x => x.PageName));
    }

    public async Task FetchNotifications()
    {
        NotificationsList.Clear();
        
        var response = await server.Send<object>(
            new HttpRequestMessage
            {
                RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Notification.Route)),
                Version = new Version("1.0"),
                Method = HttpMethod.Get
            });
        
        if (response.IsSuccessStatusCode)
        {
            // var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<NotificationDto>>>();
            // foreach (var n in content?.Payload)
            // {
            //     NotificationsList.Add(new HomeNotificationModel()
            //     {
            //         NotificationId = n.NotificationId,
            //         Subject = n.Subject,
            //         Body = n.Details,
            //         NotificationUrl = n.Link
            //     });
            // }
        }
    }
}