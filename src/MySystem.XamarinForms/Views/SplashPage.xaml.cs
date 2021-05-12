using System;
using System.Collections.Generic;
using Sayed.MySystem.ClientService.ViewModels;
using Sayed.MySystem.XamarinForms.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sayed.MySystem.XamarinForms.Views
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
