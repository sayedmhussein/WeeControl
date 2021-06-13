using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using MySystem.User.Employee.Interfaces;
using MySystem.User.Employee.Services;
using Xamarin.Forms;

namespace MySystem.User.Employee.XF
{
    public partial class App : Application
    {
        private static readonly HttpClient httpClient = new();
        private static readonly string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        

        public App()
        {
            InitializeComponent();

            try
            {
                ISharedValues sharedValues = new SharedValues();
                var device = new Services.Device(new RequestMetadata());

                httpClient.BaseAddress = new Uri(sharedValues.ApiRoute[ApiRouteEnum.Base]);
                httpClient.DefaultRequestHeaders.Add("Accept-version", sharedValues.ApiRoute[ApiRouteEnum.Version]);


                IViewModelDependencyFactory client = new ViewModelDependencyFactory(httpClient, device, appDataPath, sharedValues);

                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
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
