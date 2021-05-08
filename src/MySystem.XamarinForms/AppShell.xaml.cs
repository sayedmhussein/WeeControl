using System;
using System.Collections.Generic;
using MySystem.XamarinForms.Views;
using Xamarin.Forms;

namespace MySystem.XamarinForms
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SplashPage), typeof(SplashPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            
            
        }

    }
}
