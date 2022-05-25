using System.ComponentModel;
using WeeControl.SharedKernel.Essential;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Home;

public class HomeViewModel : ViewModelBase
{
    private readonly IUserService userService;
    private readonly IDevice device;

    public HomeViewModel(IUserService userService, IDevice device)
    {
        this.userService = userService;
        this.device = device;
    }

    public string NameOfUSer { get; private set; }

    public async Task RefreshToken()
    {
        await userService.GetTokenAsync();
        NameOfUSer = await device.Storage.GetAsync(UserDataEnum.FullName);
        //PropertyChanged.Invoke(this, nameof(NameOfUSer));
    }
}