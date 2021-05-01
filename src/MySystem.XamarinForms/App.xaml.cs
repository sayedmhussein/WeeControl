using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySystem.XamarinForms.Services;
using MySystem.XamarinForms.Views;
using MySystem.ClientService.Interfaces;

namespace MySystem.XamarinForms
{
    public partial class App : Application
    {
        //public IServiceProvider Services { get; }

        public App()
        {
            InitializeComponent();
            ApiService.InitializeClient();
            //Services = ConfigureServices();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<IDeviceInfoService, Services.Device>();
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

        //private static IServiceProvider ConfigureServices(this IServiceCollection services)
        //{
        //    var services = new ServiceCollection();

        //    services.AddSingleton<IFilesService, FilesService>();
        //    services.AddSingleton<ISettingsService, SettingsService>();
        //    services.AddSingleton<IClipboardService, ClipboardService>();
        //    services.AddSingleton<IShareService, ShareService>();
        //    services.AddSingleton<IEmailService, EmailService>();

        //    return services.BuildServiceProvider();
        //}
    }
}
