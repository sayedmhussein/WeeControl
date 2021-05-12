using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sayed.MySystem.XamarinForms.Services;
using Sayed.MySystem.XamarinForms.Views;
using Microsoft.Extensions.DependencyInjection;
using Sayed.MySystem.ClientService.ViewModels;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Sayed.MySystem.ClientService.Services;

namespace Sayed.MySystem.XamarinForms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                IDevice device = new Services.Device();
                IClientServices client = new ClientServices(device);

                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    .AddSingleton<IDevice>(device)
                    .AddSingleton<IClientServices>(client)
                    .BuildServiceProvider());
            }
            catch
            { }

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
