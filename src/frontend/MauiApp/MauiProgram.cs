using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;
using WeeControl.Frontend.AppService.Services;
using WeeControl.Frontend.MauiApp.Pages;
using WeeControl.Frontend.MauiApp.Services;

namespace WeeControl.Frontend.MauiApp;

using Microsoft.Maui.Hosting;
using WeeControl.Frontend.AppService.Interfaces;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		builder.Services.AddSingleton<IStorage, DeviceStorage>();
		builder.Services.AddSingleton<IDeviceSecurity, DeviceStorage>();
		builder.Services.AddSingleton<IDeviceServerCommunication, ServiceCommunication>();
		builder.Services.AddSingleton<IDevicePageNavigation, DeviceNavigation>();
		builder.Services.AddSingleton<IDeviceLocation, DeviceLocation>();
		builder.Services.AddSingleton<IDeviceAlert, DeviceAlert>();

		builder.Services.AddTransient<LoginPage>();

		builder.Services.AddApplicationServices();

		return builder.Build();
	}
}

