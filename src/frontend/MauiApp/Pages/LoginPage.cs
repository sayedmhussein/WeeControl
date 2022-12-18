
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

namespace WeeControl.Frontend.MauiApp.Pages;

public class LoginPage : ContentPage
{
    private readonly IAuthorizationService service;

    private readonly ScrollView scrollView;
    private readonly VerticalStackLayout stackLayout;
    private readonly Image image;
    private readonly Entry usernameEntry;
    private readonly Entry passwordEntry;
    private readonly Button loginButton;
    private readonly ActivityIndicator indicator;
    
    public LoginPage(IAuthorizationService service)
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

        Title = "Authenticating";
        Content = scrollView;
    }
    
    private void SetupGui()
    {
        scrollView.Content = stackLayout;
        
        stackLayout.WidthRequest = 400;
        stackLayout.Spacing = 25;
        stackLayout.Padding = new Thickness(30, 0);
        stackLayout.VerticalOptions = LayoutOptions.Center;
        stackLayout.Add(image);
        stackLayout.Add(usernameEntry);
        stackLayout.Add(passwordEntry);
        stackLayout.Add(loginButton);

        image.Source = "dotnet_bot.png";
        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;
        image.Aspect = Aspect.AspectFit;

        usernameEntry.Placeholder = IAuthorizationService.UsernameLabel;
        usernameEntry.Text = string.Empty;
        usernameEntry.Keyboard = Keyboard.Email;

        passwordEntry.Placeholder = IAuthorizationService.PasswordLabel;
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
        }
        
        if (string.IsNullOrWhiteSpace(passwordEntry.Text))
        {
            passwordEntry.Focus();
        }

        GuiIsBusy(true);
        
        await service.Login(usernameEntry.Text?? 
                            throw new ArgumentNullException(nameof(usernameEntry.Text), "Username can't be null!"), 
            passwordEntry.Text??
            throw new ArgumentNullException(nameof(passwordEntry.Text), "Password can't be null!"));
        
        GuiIsBusy(false);
    }

    private void GuiIsBusy(bool isBusy)
    {
        indicator.IsVisible = isBusy;
        indicator.IsEnabled = isBusy;
        indicator.IsRunning = isBusy;
        usernameEntry.IsEnabled = !isBusy;
        passwordEntry.IsEnabled = !isBusy;
        loginButton.IsEnabled = !isBusy;
    }
}