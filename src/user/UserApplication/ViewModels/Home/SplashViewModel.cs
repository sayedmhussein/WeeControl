using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Home;

public class SplashViewModel : ViewModelBase
{
    private readonly IDevice device;

    public string SplashText => "Loading Please Wait..";
    
    public SplashViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task LoadAsync()
    {
        if (await RefreshTokenAsync() && await device.Security.IsAuthenticatedAsync())
        {
            await Task.Delay(1000);
            await device.Navigation.NavigateToAsync(Pages.Home.Index);
            return;
        }
        
        await device.Navigation.NavigateToAsync(Pages.Authentication.Login);
    }
}