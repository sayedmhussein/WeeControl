namespace WeeControl.Frontend.AppService.Contexts.Essential.ViewModels;

public class AuthenticationModel
{
    public string UsernameLabel { get; set; } = "Username";
    public string Username { get; set; } = string.Empty;

    public string PasswordLabel { get; set; } = "Password";
    public string Password { get; set; } = string.Empty;

    public string LoginButtonLabel { get; set; } = "Login";
}