using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF.Views.HumanResource
{
    [QueryProperty(nameof(PassedId), "id")]
    public partial class TerritoryPage : ContentPage
    {
        public Guid PassedId { get; set; }

        public TerritoryPage()
        {
            InitializeComponent();
        }
    }
}
