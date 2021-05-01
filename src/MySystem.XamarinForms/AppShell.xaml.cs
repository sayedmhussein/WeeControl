using System;
using System.Collections.Generic;
using MySystem.XamarinForms.ViewModels;
using MySystem.XamarinForms.Views;
using Xamarin.Forms;

namespace MySystem.XamarinForms
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }

    }
}
