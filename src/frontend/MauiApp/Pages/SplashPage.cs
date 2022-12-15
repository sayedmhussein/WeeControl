namespace WeeControl.Frontend.MauiApp.Pages;

public class SplashPage : ContentPage
{
    private readonly LoginPage loginPage;

    public SplashPage(LoginPage loginPage)
    {
        this.loginPage = loginPage;
        var image = new Image()
        {
            Source = "company_logo.png",
            HeightRequest = 200,
            HorizontalOptions = LayoutOptions.Center
        };
        SemanticProperties.SetDescription(image, "Welcome to WeeControl App");

        var label = new Label()
        {
            Text = "Please wait",
            FontSize = 32,
            HorizontalOptions = LayoutOptions.Center
        };

        var indicator = new ActivityIndicator()
        {
            IsRunning = true
        };

        var stack = new VerticalStackLayout()
        {
            Spacing = 25,
            Padding = new Thickness(30, 0),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };
        stack.Add(image);
        stack.Add(label);
        stack.Add(indicator);

        Title = "Splash...";
        Content = stack;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(3000);
        await Navigation.PushModalAsync(loginPage);
    }
}