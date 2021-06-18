using WeeControl.Applications.Employee.XF.Views;
using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(SplashPage), typeof(SplashPage));
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));

            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));


        }

    }
}
