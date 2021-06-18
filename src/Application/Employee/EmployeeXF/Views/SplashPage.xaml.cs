using WeeControl.Applications.BaseLib.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeControl.Applications.Employee.XF.Views
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
            base.OnAppearing();
            await vm.RefreshTokenCommand.ExecuteAsync(null);
        }
    }
}
