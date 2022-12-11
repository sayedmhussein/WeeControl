namespace WeeControl.Frontend.MauiApp.Pages;

public partial class SplashPage : ContentPage
{
	int count = 0;

	public SplashPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private async void NavBtn_OnClicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new AuthenticationPage());
	}
}


