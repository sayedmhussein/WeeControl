namespace WeeControl.Frontend.MauiApp.Pages;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        var text = await DisplayPromptAsync("Feedback", "Sending Feedback to the developer", "Send", "Cancel", "Please write your feedback then press send");
        await DisplayAlert("Feedback Sent", text, "Thanks");
    }
}