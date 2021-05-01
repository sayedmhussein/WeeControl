using System;
using System.Collections.Generic;
using MySystem.ClientService.Interfaces;
using MySystem.ClientService.ViewModels;
using Xamarin.Forms;

namespace MySystem.XamarinForms.Views
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            var vm = (SplashViewModel)BindingContext;
            vm.DeviceInfo = DependencyService.Get<IDeviceInfoService>();
            //BindingContext = Application.Current.Services.GetService<ContactsViewModel>();
        }
    }
}
