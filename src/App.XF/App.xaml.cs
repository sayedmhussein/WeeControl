using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySystem.Persistence.XamarinForms.Services;
using MySystem.Persistence.XamarinForms.Views;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Persistence.ClientService.ViewModels;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using MySystem.Persistence.ClientService.Services;
using MySystem.Persistence.Shared.Configuration.Models;
using MySystem.Persistence.Shared.Configuration;

namespace MySystem.Persistence.XamarinForms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                IDevice device = new Services.Device();
                IApi api = AppSettings.GetAppSetting().Api;
                IClientServices client = new ClientServices(device, api);

                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    .AddSingleton(device)
                    .AddSingleton(client)
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
