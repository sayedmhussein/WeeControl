using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySystem.Web.XamarinForms.Services;
using MySystem.Web.XamarinForms.Views;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Web.ClientService.ViewModels;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using MySystem.Web.ClientService.Services;
using MySystem.Web.Shared.Configuration.Models;
using MySystem.Web.Shared.Configuration;

namespace MySystem.Web.XamarinForms
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
