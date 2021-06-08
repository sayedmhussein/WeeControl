using System;
using System.Collections.Generic;
using MySystem.Persistence.ClientService.ViewModels;
using MySystem.Persistence.XamarinForms.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MySystem.Persistence.XamarinForms.Views
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
