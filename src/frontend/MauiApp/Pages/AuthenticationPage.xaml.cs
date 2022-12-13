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

    private void Button_OnClicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new AppShell();
        
    }
}