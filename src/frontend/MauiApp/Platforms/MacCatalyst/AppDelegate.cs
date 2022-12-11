using Foundation;
using WeeControl.Frontend.MauiApp;
using Microsoft.Maui.Hosting;

namespace WeeControl.Frontend.MauiApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override Microsoft.Maui.Hosting.MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

