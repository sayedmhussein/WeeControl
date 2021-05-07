using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySystem.XamarinForms.Services;
using MySystem.XamarinForms.Views;
using MySystem.ClientService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MySystem.ClientService.ViewModels;
using System.IO;
using System.Reflection;
using MySystem.XamarinForms.Models;
using Newtonsoft.Json;

namespace MySystem.XamarinForms
{
    public partial class Application : Xamarin.Forms.Application
    {
        //public IServiceProvider Services { get; }
        //private static Stream resourceStream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("appsettings.json");
        private static AppSettings appSetting;
        public static AppSettings AppSettings
        {
            get
            {
                if (appSetting == null)
                    LoadAppSetting();

                return appSetting;
            }
        }

        public Application()
        {
            InitializeComponent();
            DeviceResources.InitializeClient();
            //Services = ConfigureServices();
            //LoadAppSetting();

            DependencyService.RegisterSingleton<IAppSettings>(AppSettings);


            //DependencyService.Register<MockDataStore>();
            DependencyService.RegisterSingleton<IDeviceInfo>(new DeviceInfo());
            //DependencyService.Register<DeviceActions>();
            //DependencyService.Register<IDeviceActions, DeviceActions>();
            //DependencyService.Register<IApiUri, ApiUri>();
            MainPage = new SplashPage();
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

        private static void LoadAppSetting()
        {
#if DEBUG
            var appsettingResouceStream = Assembly.GetAssembly(typeof(AppSettings)).GetManifestResourceStream("MySystem.XamarinForms.Configuration.appsettings.debug.json");
#endif
            if (appsettingResouceStream == null)
                return;

            using (var streamReader = new StreamReader(appsettingResouceStream))
            {
                var jsonStream = streamReader.ReadToEnd();
                appSetting = JsonConvert.DeserializeObject<AppSettings>(jsonStream);
            }
            
        }

        //private static IServiceProvider ConfigureServices()
        //{
        //    var services = new ServiceCollection();

        //    //services.AddSingleton<IDeviceInfo, DeviceInfo>();
        //    //Add more services here...
            
            

        //    //services.AddTransient<SplashViewModel>();

        //    return services.BuildServiceProvider();
        //}
    }
}
