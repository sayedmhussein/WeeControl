using WeeControl.Applications.BaseLib.ViewModels.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeControl.Applications.Employee.XF.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        private readonly SplashViewModel vm;

        public SplashPage()
        {
            InitializeComponent();
            
            vm = (SplashViewModel)BindingContext;
        }


        async protected override void OnAppearing()
        {
            await vm.RefreshTokenCommand.ExecuteAsync(null);
            base.OnAppearing();
            
        }
    }
}
