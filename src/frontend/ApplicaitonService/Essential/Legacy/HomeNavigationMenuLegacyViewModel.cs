using WeeControl.Frontend.ApplicationService.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;

namespace WeeControl.Frontend.ApplicationService.Essential.Legacy;

[Obsolete("Use HomeViewModel.")]
public class HomeNavigationMenuLegacyViewModel : LegacyViewModelBase
{
    private readonly IDevice device;

    public IEnumerable<MenuItemModel> MenuItems { get; private set; } = new List<MenuItemModel>();

    public HomeNavigationMenuLegacyViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }
    
    public Task ChangeMyPasswordAsync()
    {
        return device.Navigation.NavigateToAsync(Pages.Essential.User.SetNewPasswordPage);
    }

    public async Task SetupMenuAsync()
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

        MenuItems = new List<MenuItemModel>(list.DistinctBy(x => x.PageName));
    }
}