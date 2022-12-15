using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

namespace WeeControl.Frontend.MauiApp.Services;

public class DeviceAlert : IDeviceAlert
{
    public Task DisplayAlert(string message)
    {
        return App.Current.MainPage.DisplayAlert("", message, "ok");
    }
}