using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF.Views.HumanResource
{
    [QueryProperty(nameof(Name), "name")]
    public partial class TerritoryDetailPage : ContentPage
    {
        public string Name { set { Bla.Text = value; } }

        public TerritoryDetailPage()
        {
            InitializeComponent();
        }
    }
}
