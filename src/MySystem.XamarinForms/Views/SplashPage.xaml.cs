using System;
using System.Collections.Generic;
using MySystem.ClientService.Interfaces;
using MySystem.ClientService.ViewModels;
using MySystem.XamarinForms.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MySystem.XamarinForms.Views
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
