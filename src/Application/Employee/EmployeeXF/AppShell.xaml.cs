using WeeControl.Applications.BaseLib.ViewModels;
using WeeControl.Applications.Employee.XF.Views;
using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF
{
    public partial class AppShell : Shell
    {
        //private readonly ShellViewModel vm;

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SplashPage), typeof(SplashPage));
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));

            //Routing.RegisterRoute("HumanResource", typeof(Views.HumanResource.HomePage));
            Routing.RegisterRoute("HumanResource/TerritoryDetailPage", typeof(Views.HumanResource.TerritoryDetailPage));

            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //vm = (ShellViewModel)BindingContext;
        }
    }
}
