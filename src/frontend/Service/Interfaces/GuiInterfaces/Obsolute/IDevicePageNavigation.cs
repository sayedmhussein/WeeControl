namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

public interface IDevicePageNavigation
{
    Task NavigateToAsync(string pageName, bool forceLoad = false);

    Task GoBackAsync();
}