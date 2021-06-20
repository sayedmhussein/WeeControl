using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.Services;
using WeeControl.Applications.Employee.XF.Services;
using WeeControl.SharedKernel.BasicSchemas.Common.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Territory.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Territory.Interfaces;
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


                IViewModelDependencyFactory debFactory = new ViewModelDependencyFactory(httpClient, device, appDataPath);

                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    .AddSingleton<IAppDatabase>(new AppDatabase(Path.Combine(appDataPath, "Notes.db3")))
                    .AddSingleton(debFactory)
                    .AddSingleton(apiDicts)

                    .AddSingleton<ITerritoryDicts, TerritoryDicts>()
                    //
                    .AddSingleton<IClaimDicts, ClaimDicts>()
                    .AddSingleton<IIdentityDicts, IdentityDicts>()
                    .AddSingleton<IPersonalAttribDicts, PersonalAttribDicts>()

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
