using WeeControl.Applications.BaseLib.ViewModels.Common;
using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF.Views.Common
{
    public partial class AppShell : Shell
    {
        private readonly ShellViewModel vm;

        public AppShell()
        {
            InitializeComponent();
            vm = (ShellViewModel)BindingContext;

            Routing.RegisterRoute("HumanResource/EmployeeDetailPage", typeof(Views.HumanResource.EmployeePage));
            Routing.RegisterRoute("HumanResource/TerritoryDetailPage", typeof(Views.HumanResource.TerritoryPage));
        }

        protected async override void OnAppearing()
        {
            await vm.RefreshCommand.ExecuteAsync(null);
            base.OnAppearing();
        }

        
    }
}
