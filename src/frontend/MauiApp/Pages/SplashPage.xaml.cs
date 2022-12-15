namespace WeeControl.Frontend.MauiApp.Pages;

public partial class SplashPage : ContentPage
{
	private readonly LoginPage loginPage;

	public SplashPage(LoginPage loginPage)
	{
		this.loginPage = loginPage;
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await Task.Delay(3000);
		await Navigation.PushModalAsync(loginPage);
	}
}


