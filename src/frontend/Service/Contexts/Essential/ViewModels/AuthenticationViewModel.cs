using System.Windows.Input;

namespace WeeControl.Frontend.AppService.Contexts.Essential.ViewModels;

public class AuthenticationViewModel : ViewModelBase
{
    private readonly AuthenticationModel model = new();

    public string UsernameLabel => model.UsernameLabel;
    public string Username
    {
        get => model.Username;
        set
        {
            model.Username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public string PasswordLabel => model.PasswordLabel;
    public string Password
    {
        get => model.Password;
        set
        {
            model.Password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public string LoginButtonLabel => model.LoginButtonLabel;
    
    public ICommand LoginCommand { get; }

    public AuthenticationViewModel()
    {
        
    }

    public async Task LoginAsync()
    {
        await Task.Delay(5000);
        
        UserIsAuthorized?.Invoke(this, true);
    }
    
    public event EventHandler<bool>? UserIsAuthorized;
}