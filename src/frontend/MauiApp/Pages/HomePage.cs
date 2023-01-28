namespace WeeControl.Frontend.MauiApp.Pages;

public class HomePage : ContentPage
{
    public HomePage()
    {
        var feedStack = new VerticalStackLayout();

        var scroll = new ScrollView
        {
            Content = feedStack
        };

        Content = scroll;
    }
}