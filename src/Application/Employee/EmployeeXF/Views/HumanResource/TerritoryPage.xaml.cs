using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF.Views.HumanResource
{
    public partial class TerritoryPage : ContentPage
    {
        public TerritoryPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("TerritoryDetailPage?name=sayed");
        }
    }
}
