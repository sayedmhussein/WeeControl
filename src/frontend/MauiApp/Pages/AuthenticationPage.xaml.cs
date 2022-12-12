using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeControl.Frontend.MauiApp.Pages;

public partial class AuthenticationPage : ContentPage
{
    public AuthenticationPage()
    {
        InitializeComponent();
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Title", "Navigating to other Page", "Ok");
        App.Current.MainPage = new AppShell();
    }
}