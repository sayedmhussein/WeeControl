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

            vm.DeviceAction = new DeviceActions();
            vm.ApiUri = new ApiUri();
            
            //BindingContext = Application.Current.Services.GetService<ContactsViewModel>();
        }

        protected override void OnAppearing()
        {
            vm.RefreshTokenCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
