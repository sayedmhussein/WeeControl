using WeeControl.Applications.BaseLib.ViewModels.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeeControl.Applications.Employee.XF.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel vm;

        public HomePage()
        {
            InitializeComponent();
            vm = (HomeViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            //vm.RefreshTokenCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
