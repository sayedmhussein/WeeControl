using WeeControl.Frontend.AppService;
using WeeControl.Frontend.MauiApp.Pages;
using WeeControl.Frontend.MauiApp.Services;

namespace WeeControl.Frontend.MauiApp;

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

        builder.Services.AddSingleton<IDeviceData, GuiServices>();

        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddApplicationServices();

        return builder.Build();
    }
}

