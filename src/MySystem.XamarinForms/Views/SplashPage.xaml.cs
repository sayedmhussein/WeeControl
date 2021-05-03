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
        public SplashPage()
        {
            InitializeComponent();
            var vm = (SplashViewModel)BindingContext;
            vm.DeviceInfo = new DeviceInfo();
            vm.DeviceAction = new DeviceActions();
            vm.DeviceResources = new DeviceResources();
            vm.ApiUri = new ApiUri();
            vm.RefreshTokenCommand.Execute(null);
            //BindingContext = Application.Current.Services.GetService<ContactsViewModel>();
        }
    }
}
