using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.Services;
using WeeControl.Applications.Employee.XF.Services;
using WeeControl.Applications.Employee.XF.Views.Common;
using WeeControl.SharedKernel.BasicSchemas.Common;
using WeeControl.SharedKernel.BasicSchemas.Employee;
using WeeControl.SharedKernel.BasicSchemas.Territory;
using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF
{
    public partial class App : Application
    {
        private static readonly string appDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public App()
        {
            InitializeComponent();

            try
            {
                ICommonLists apiDicts = new CommonLists();
                var device = new Services.Device();
                var serviceCollection = GetServiceCollection(device, apiDicts);

                Ioc.Default.ConfigureServices(serviceCollection.BuildServiceProvider()) ;
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

        private IServiceCollection GetServiceCollection(IDevice device, ICommonLists apiDicts)
        {
            return new ServiceCollection()
                    .AddSingleton<IServerService>(new ServerService(device, device, device, device, apiDicts))
                    .AddSingleton<IDevice>(device)
                    .AddSingleton<IBasicDatabase>(new AppDatabase(Path.Combine(appDataPath, "basic.db3")))

                    .AddSingleton<ICommonLists>(apiDicts)
                    //
                    .AddSingleton<ITerritoryLists, TerritoryLists>()
                    //
                    .AddSingleton<IEmployeeLists, EmployeeLists>();
        }
    }
}
