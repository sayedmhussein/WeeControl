using WeeControl.Frontend.MauiApp.Pages;

namespace WeeControl.Frontend.MauiApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("AboutPage", typeof(AboutPage));
	}

    async void AboutItem_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("AboutPage");
    }

    async void MenuItem_Clicked_1(System.Object sender, System.EventArgs e)
    {
        await DisplayAlert("Subject", "Clicked Logout", "Ok");
    }
}

