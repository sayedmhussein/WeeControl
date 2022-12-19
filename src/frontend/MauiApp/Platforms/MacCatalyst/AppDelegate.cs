using Microsoft.Maui;
using Foundation;

namespace WeeControl.Frontend.MauiApp.Platforms.MacCatalyst;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override Microsoft.Maui.Hosting.MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

