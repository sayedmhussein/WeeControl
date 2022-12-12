using System.Windows.Input;

namespace WeeControl.Frontend.AppService.Contexts.Essential.ViewModels;

public class AuthenticationViewModel : ViewModelBase
{
    private string username = string.Empty;
    public string Username
    {
        get => username;
        set
        {
            username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    
    private string password = string.Empty;
    public string Password
    {
        get => password;
        set
        {
            password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public AuthenticationViewModel()
    {
        
    }

    public Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            DisplayAlert("Invalid username!");
        }
        DisplayAlert("Hello World");
        return Task.CompletedTask;
    }
}