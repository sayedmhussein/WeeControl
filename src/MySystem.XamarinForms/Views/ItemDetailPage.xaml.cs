using System.ComponentModel;
using Xamarin.Forms;
using MySystem.XamarinForms.ViewModels;

namespace MySystem.XamarinForms.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}