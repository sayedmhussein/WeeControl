using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Essential.Legacy;

[Obsolete("Use HomeViewModel Init() function.")]
public class SplashLegacyViewModel : LegacyViewModelBase
{
    private readonly IDevice device;

    public string SplashText => "Loading Please Wait..";
    
    public SplashLegacyViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task LoadAsync()
    {
        if (await RefreshTokenAsync() && await device.Security.IsAuthenticatedAsync())
        {
            await Task.Delay(3000);
            await device.Navigation.NavigateToAsync(Pages.Shared.IndexPage);
            return;
        }
        
        await device.Navigation.NavigateToAsync(Pages.Essential.Authentication.LoginPage);
    }
}