
/* Unmerged change from project 'MauiApp (net7.0-ios)'
Before:
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
After:
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
*/

/* Unmerged change from project 'MauiApp (net7.0-maccatalyst)'
Before:
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
After:
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
*/

/* Unmerged change from project 'MauiApp (net7.0-windows10.0.19041.0)'
Before:
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
After:
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
*/

using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

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

        var label = new IAuthorizationService.Label()
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