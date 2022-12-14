using Microsoft.Maui.Controls.Compatibility;
using WeeControl.Frontend.AppService.Contexts.Essential.Interfaces;
using WeeControl.Frontend.AppService.Contexts.Essential.Models;

namespace WeeControl.Frontend.MauiApp.Pages;

public class LoginPage : ContentPage
{
    private readonly IUserAuthorizationService service;

    private readonly ScrollView scrollView;
    private readonly VerticalStackLayout stackLayout;
    private readonly Image image;
    private readonly Entry usernameEntry;
    private readonly Entry passwordEntry;
    private readonly Button loginButton;
    private readonly ActivityIndicator indicator;
    
    public LoginPage(IUserAuthorizationService service)
    {
        this.service = service;

        scrollView = new ScrollView();
        stackLayout = new VerticalStackLayout();
        image = new Image();
        usernameEntry = new Entry();
        passwordEntry = new Entry();
        loginButton = new Button();
        indicator = new ActivityIndicator();
        
        SetupGui();
        GuiIsBusy(false);
    }
    
    private void SetupGui()
    {
        scrollView.Content = stackLayout;
        
        stackLayout.WidthRequest = 400;
        stackLayout.Add(image);
        stackLayout.Add(usernameEntry);
        stackLayout.Add(passwordEntry);
        stackLayout.Add(loginButton);

        image.Source = "dotnet_bot.png";
        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;
        image.Aspect = Aspect.AspectFit;

        usernameEntry.Placeholder = "Username";
        usernameEntry.Text = string.Empty;
        usernameEntry.Keyboard = Keyboard.Email;

        passwordEntry.Placeholder = "Password";
        passwordEntry.Text = string.Empty;
        passwordEntry.IsPassword = true;

        loginButton.Text = "Login";
        loginButton.Clicked += LoginButtonOnClicked;
    }

    private async void LoginButtonOnClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(usernameEntry.Text))
        {
            usernameEntry.Focus();
            usernameEntry.BackgroundColor = Colors.LightYellow;
        }
        
        if (string.IsNullOrWhiteSpace(passwordEntry.Text))
        {
            passwordEntry.Focus();
            passwordEntry.BackgroundColor = Colors.LightYellow;
        }

        GuiIsBusy(true);
        
        await service.Login(new LoginModel()
        {
            UsernameOrEmail = usernameEntry.Text?? string.Empty, 
            Password = passwordEntry.Text ?? string.Empty
        });
        
        GuiIsBusy(false);
    }

    private void GuiIsBusy(bool isBusy)
    {
        indicator.IsVisible = isBusy;
        usernameEntry.IsEnabled = !isBusy;
        passwordEntry.IsEnabled = !isBusy;
        loginButton.IsEnabled = !isBusy;
    }
}