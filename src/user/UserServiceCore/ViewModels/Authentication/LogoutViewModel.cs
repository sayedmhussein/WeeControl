using System.ComponentModel;
using WeeControl.SharedKernel.Essential;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Authentication;

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

    public Task LogoutAsync()
    {
        return userService.LogoutAsync();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}