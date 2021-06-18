using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.Services;
using WeeControl.SharedKernel.CommonSchemas.Common.Dicts;
using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;
using WeeControl.SharedKernel.CommonSchemas.Common.Enums;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;
using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF
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
                IApiDicts apiDicts = new ApiDicts();
                var device = new Services.Device(new RequestMetadata());

                httpClient.BaseAddress = new Uri(apiDicts.ApiRoute[ApiRouteEnum.Base]);
                httpClient.DefaultRequestHeaders.Add("Accept-version", apiDicts.ApiRoute[ApiRouteEnum.Version]);


                IViewModelDependencyFactory client = new ViewModelDependencyFactory(httpClient, device, appDataPath);

                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    .AddSingleton(client)
                    //.AddSingleton<ITerritoryValues>(new TerritoryValues())
                    //.AddSingleton<IEmployeeValues>(new EmployeeValues())
                    .AddSingleton(apiDicts)
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
