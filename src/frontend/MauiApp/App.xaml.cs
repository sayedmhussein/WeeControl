using WeeControl.Frontend.MauiApp.Pages;

namespace WeeControl.Frontend.MauiApp;

public partial class App : Application
{
	private readonly LoginPage loginPage;

	public App(LoginPage loginPage)
	{
		this.loginPage = loginPage;
		InitializeComponent();

		MainPage = new SplashPage(loginPage);
	}
}

