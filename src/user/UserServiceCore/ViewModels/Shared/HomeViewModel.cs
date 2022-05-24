using System.ComponentModel;
using WeeControl.SharedKernel.Essential;

namespace WeeControl.User.UserServiceCore.ViewModels.Shared;

public class HomeViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;

    public HomeViewModel(IUserService userService)
    {
        this.userService = userService;
    }

    public Task RefreshToken()
    {
        return userService.GetTokenAsync();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}