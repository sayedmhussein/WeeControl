using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using MySystem.User.Employee.Interfaces;
using MySystem.User.Employee.Services;
using Xamarin.Forms;

namespace MySystem.User.Employee.XF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                var device = new Services.Device();
                ISharedValues sharedValues = new SharedValues();
                IViewModelDependencyFactory client = new ViewModelDependencyFactory(device, sharedValues);

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
