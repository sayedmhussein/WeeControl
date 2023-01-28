
/* Unmerged change from project 'MauiApp (net7.0-ios)'
Before:
using System;
using Microsoft.Maui.Controls;
After:
using Microsoft.Maui.Controls;
using System;
*/

/* Unmerged change from project 'MauiApp (net7.0-maccatalyst)'
Before:
using System;
using Microsoft.Maui.Controls;
After:
using Microsoft.Maui.Controls;
using System;
*/

/* Unmerged change from project 'MauiApp (net7.0-windows10.0.19041.0)'
Before:
using System;
using Microsoft.Maui.Controls;
After:
using Microsoft.Maui.Controls;
using System;
*/
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