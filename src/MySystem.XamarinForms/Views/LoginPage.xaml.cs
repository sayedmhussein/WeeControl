using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sayed.MySystem.ClientService.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sayed.MySystem.XamarinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel vm;

        public LoginPage()
        {
            InitializeComponent();
            vm = (LoginViewModel)BindingContext;
            AppIsBusy = false;
        }

        void UsernameEntry_Completed(Object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }

        void PasswordEntry_Completed(Object sender, EventArgs e)
        {
            LoginButton_Clicked(this, null);
        }

        async void LoginButton_Clicked(Object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text))
            {
                UsernameEntry.Focus();
                UsernameEntry.BackgroundColor = Color.LightYellow;
                await DisplayAlert("Username", "You didn't entered a valid username", "OK");
            }
            else if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                UsernameEntry.BackgroundColor = Color.Default;
                PasswordEntry.Focus();
                PasswordEntry.BackgroundColor = Color.LightYellow;
                await DisplayAlert("Password", "You didn't entered a valid password", "OK");
            }
            else
            {
                AppIsBusy = true;
                await vm.LoginCommand.ExecuteAsync(new object());
                AppIsBusy = false;
            }
        }

        private bool AppIsBusy
        {
            set
            {
                if (value)
                {
                    ActivityIndicatorRunning.IsVisible = true;
                    EntryStack.IsEnabled = false;
                }
                else
                {
                    ActivityIndicatorRunning.IsVisible = false;
                    EntryStack.IsEnabled = true;
                }
            }
        }
    }
}