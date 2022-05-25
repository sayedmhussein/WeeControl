using WeeControl.SharedKernel.Essential;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Home;

public class SplashViewModel : ViewModelBase
{
    private readonly IUserService userService;
    private readonly IDevice device;

    public string SplashText => "Loading Please Wait..";
    
    public SplashViewModel(IUserService userService, IDevice device)
    {
        this.userService = userService;
        this.device = device;
    }

    public async Task LoadAsync()
    {
        if (await device.Security.IsAuthenticatedAsync())
        {
            await Task.Delay(1000);
            await userService.GetTokenAsync();
            await device.Navigation.NavigateToAsync(PagesEnum.Home);
        }
        else
        {
            await device.Navigation.NavigateToAsync(PagesEnum.Login);
        }
    }
}