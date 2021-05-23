using System;
using System.Collections.Generic;
using MySystem.Web.ClientService.ViewModels;
using MySystem.Web.XamarinForms.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MySystem.Web.XamarinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        private readonly SplashViewModel vm;

        public SplashPage()
        {
            InitializeComponent();
            
            vm = (SplashViewModel)BindingContext;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await vm.RefreshTokenCommand.ExecuteAsync(null);
        }
    }
}
