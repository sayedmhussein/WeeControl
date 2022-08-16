using WeeControl.Frontend.ApplicationService.Contexts.Anonymous.Interfaces;
using WeeControl.Frontend.ApplicationService.Contexts.Customer.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.Frontend.ApplicationService.Contexts.Common;

internal class UserHomeViewModel : ViewModelBase, IUserHomeViewModel
{
    private readonly IDevice device;
    private readonly IServerOperation server;
    private readonly IAuthorizationViewModel authorizationViewModel;

    public string GreetingMessage { get; private set; } = "Hello";
    public bool IsEmployee { get; private set; }
    public bool IsCustomer { get; private set; }
    public List<MenuItemModel> MenuItems { get; }
    public List<HomeFeedModel> FeedsList { get; }
    public List<HomeNotificationModel> NotificationsList { get; }

    public UserHomeViewModel(IDevice device, IServerOperation server, IAuthorizationViewModel authorizationViewModel)
    {
        this.device = device;
        this.server = server;
        this.authorizationViewModel = authorizationViewModel;
        MenuItems = new List<MenuItemModel>();
        FeedsList = new List<HomeFeedModel>();
        NotificationsList = new List<HomeNotificationModel>();
    }

    public async Task Init()
    {
        if (await authorizationViewModel.IsAuthorized())
        {
            await Refresh();
        }

        await device.Navigation.NavigateToAsync(Pages.Anonymous.IndexPage, forceLoad:true);
    }

    public async Task ChangeMyPasswordAsync()
    {
        //return device.Navigation.NavigateToAsync(Pages.Anonymous.SetNewPasswordPage);
    }

    private async Task SetupMenuAsync()
    {
        var list = new List<MenuItemModel>();

        foreach (var claim in await device.Security.GetClaimsAsync())
        {
            if (claim.Type is ClaimsValues.ClaimTypes.Session or ClaimsValues.ClaimTypes.Territory or ClaimsValues.ClaimTypes.Country)
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

    public async Task Refresh()
    {
        await SetupMenuAsync();
        
        GreetingMessage = "Hello " + await device.Storage.GetAsync(nameof(AuthenticationResponseDto.FullName));
        var claims = await device.Security.GetClaimsAsync();
        if (claims.FirstOrDefault(x => x.Type == ClaimsValues.ClaimTypes.Territory)?.Value is not null)
        {
            IsEmployee = true;
        }
            
        if (claims.FirstOrDefault(x => x.Type == ClaimsValues.ClaimTypes.Country)?.Value is not null)
        {
            IsCustomer = true;
        }
        
        NotificationsList.Clear();
        
        // var response = await server.Send<object>(
        //     new HttpRequestMessage
        //     {
        //         RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Customer)),
        //         Version = new Version("1.0"),
        //         Method = HttpMethod.Get
        //     });
        //
        // if (response.IsSuccessStatusCode)
        // {
        //     // var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<NotificationDto>>>();
        //     // foreach (var n in content?.Payload)
        //     // {
        //     //     NotificationsList.Add(new HomeNotificationModel()
        //     //     {
        //     //         NotificationId = n.NotificationId,
        //     //         Subject = n.Subject,
        //     //         Body = n.Details,
        //     //         NotificationUrl = n.Link
        //     //     });
        //     // }
        // }
    }
}