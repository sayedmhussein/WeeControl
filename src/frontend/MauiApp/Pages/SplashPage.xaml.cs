namespace WeeControl.Frontend.MauiApp.Pages;

public partial class SplashPage : ContentPage
{
	public SplashPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await Task.Delay(3000);
		await Navigation.PushModalAsync(new LoginPage(null));
	}
}


