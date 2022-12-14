using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeControl.Frontend.AppService.Contexts.Essential.Interfaces;
using WeeControl.Frontend.AppService.Contexts.Essential.ViewModels;

namespace WeeControl.Frontend.MauiApp.Pages;

public partial class AuthenticationPage : ContentPage
{
    private readonly IUserAuthorizationService service;

    public AuthenticationPage(IUserAuthorizationService service)
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