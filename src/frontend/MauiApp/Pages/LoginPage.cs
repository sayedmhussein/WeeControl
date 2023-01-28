
/* Unmerged change from project 'MauiApp (net7.0-ios)'
Before:
using System;
using Microsoft.Maui;
After:
using Microsoft.Maui;
*/

/* Unmerged change from project 'MauiApp (net7.0-maccatalyst)'
Before:
using System;
using Microsoft.Maui;
After:
using Microsoft.Maui;
*/

/* Unmerged change from project 'MauiApp (net7.0-windows10.0.19041.0)'
Before:
using System;
using Microsoft.Maui;
After:
using Microsoft.Maui;
*/
using
/* Unmerged change from project 'MauiApp (net7.0-ios)'
Before:
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
After:
using System;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
*/

/* Unmerged change from project 'MauiApp (net7.0-maccatalyst)'
Before:
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
After:
using System;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
*/

/* Unmerged change from project 'MauiApp (net7.0-windows10.0.19041.0)'
Before:
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
After:
using System;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
*/
WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

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

    /* Unmerged change from project 'MauiApp (net7.0-ios)'
    Before:
        private readonly ActivityIndicator indicator;

        public LoginPage(IAuthorizationService service)
    After:
        private readonly ActivityIndicator indicator;

        public LoginPage(IAuthorizationService service)
    */

    /* Unmerged change from project 'MauiApp (net7.0-maccatalyst)'
    Before:
        private readonly ActivityIndicator indicator;

        public LoginPage(IAuthorizationService service)
    After:
        private readonly ActivityIndicator indicator;

        public LoginPage(IAuthorizationService service)
    */

    /* Unmerged change from project 'MauiApp (net7.0-windows10.0.19041.0)'
    Before:
        private readonly ActivityIndicator indicator;

        public LoginPage(IAuthorizationService service)
    After:
        private readonly ActivityIndicator indicator;

        public LoginPage(IAuthorizationService service)
    */
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

        /* Unmerged change from project 'MauiApp (net7.0-ios)'
        Before:
                scrollView.Content = stackLayout;

                stackLayout.WidthRequest = 400;
        After:
                scrollView.Content = stackLayout;

                stackLayout.WidthRequest = 400;
        */

        /* Unmerged change from project 'MauiApp (net7.0-maccatalyst)'
        Before:
                scrollView.Content = stackLayout;

                stackLayout.WidthRequest = 400;
        After:
                scrollView.Content = stackLayout;

                stackLayout.WidthRequest = 400;
        */

        /* Unmerged change from project 'MauiApp (net7.0-windows10.0.19041.0)'
        Before:
                scrollView.Content = stackLayout;

                stackLayout.WidthRequest = 400;
        After:
                scrollView.Content = stackLayout;

                stackLayout.WidthRequest = 400;
        */
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

        usernameEntry.Placeholder = service.GetLabel(IAuthorizationService.Label.Username);
        usernameEntry.Text = string.Empty;
        usernameEntry.Keyboard = Keyboard.Email;

        passwordEntry.Placeholder = service.GetLabel(IAuthorizationService.Label.Password);
        passwordEntry.Text = string.Empty;
        passwordEntry.IsPassword = true;

        loginButton.Text = service.GetLabel(IAuthorizationService.Label.LoginButton);
        loginButton.Clicked += LoginButtonOnClicked;
    }

    private async void LoginButtonOnClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(usernameEntry.Text))
        {
            usernameEntry.Focus();

            /* Unmerged change from project 'MauiApp (net7.0-ios)'
            Before:
                    }

                    if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            After:
                    }

                    if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            */

            /* Unmerged change from project 'MauiApp (net7.0-maccatalyst)'
            Before:
                    }

                    if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            After:
                    }

                    if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            */

            /* Unmerged change from project 'MauiApp (net7.0-windows10.0.19041.0)'
            Before:
                    }

                    if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            After:
                    }

                    if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            */
        }

        if (string.IsNullOrWhiteSpace(passwordEntry.Text))
        {
            passwordEntry.Focus();
        }

        GuiIsBusy(true);

        await service.Login(usernameEntry.Text ??
                            throw new ArgumentNullException(nameof(usernameEntry.Text), "Username can't be null!"),
            passwordEntry.Text ??
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