using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

namespace WeeControl.Frontend.MauiApp.Services;

[Obsolete("Use GuiService class", true)]
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