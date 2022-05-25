using System.ComponentModel;
using WeeControl.SharedKernel.Essential;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Essentials;

public class LogoutViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;
    private readonly IDevice device;

    public string GoodbyMessage => "Good By :)";
    
    public LogoutViewModel(IUserService userService, IDevice device)
    {
        this.userService = userService;
        this.device = device;
    }

    public async Task LogoutAsync()
    {
        await userService.LogoutAsync();
        await device.Navigation.NavigateToAsync(PagesEnum.Login, forceLoad: true);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}