using System;
using System.Collections.Generic;
using Sayed.MySystem.XamarinForms.Views;
using Xamarin.Forms;

namespace Sayed.MySystem.XamarinForms
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(SplashPage), typeof(SplashPage));
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));

            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));


        }

    }
}
