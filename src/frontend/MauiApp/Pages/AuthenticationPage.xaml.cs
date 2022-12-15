using WeeControl.Frontend.AppService.AppInterfaces;

namespace WeeControl.Frontend.MauiApp.Pages;

public partial class AuthenticationPage : ContentPage
{
    private readonly IAuthorizationService service;

    public AuthenticationPage(IAuthorizationService service)
    {
        this.service = service;
        InitializeComponent();
        SetupGui();
    }

    private void SetupGui()
    {
        UsernameEntry.Placeholder = "Username";
        PasswordEntry.Placeholder = "Password";
        LoginButton.Text = "Login";
    }
}