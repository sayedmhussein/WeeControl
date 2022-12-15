using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.MauiApp.Services;

public class DeviceNavigation : IDevicePageNavigation
{
    public Task NavigateToAsync(string pageName, bool forceLoad = false)
    {
        return Shell.Current.GoToAsync("//" + pageName);
    }

    public Task GoBackAsync()
    {
        throw new NotImplementedException();
    }
}