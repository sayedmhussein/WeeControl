using System;
using System.Collections.Generic;
using MySystem.ClientService.Interfaces;
using MySystem.ClientService.ViewModels;
using MySystem.XamarinForms.Services;
using Xamarin.Forms;

namespace MySystem.XamarinForms.Views
{
    public partial class SplashPage : ContentPage
    {
        private readonly SplashViewModel vm;

        public SplashPage()
        {
            InitializeComponent();
            
            vm = (SplashViewModel)BindingContext;
           //vm.AppSettings = DependencyService.Get<IAppSettings>();
            //vm.DeviceInfo = DependencyService.Get<IDeviceInfo>();
            vm.DeviceAction = new DeviceActions();
            vm.DeviceResources = new DeviceResources();
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
