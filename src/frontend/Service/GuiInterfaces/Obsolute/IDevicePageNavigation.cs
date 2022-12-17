namespace WeeControl.Frontend.AppService.GuiInterfaces.Obsolute;

[Obsolete("Use IGui instead.", true)]
public interface IDevicePageNavigation
{
    Task NavigateToAsync(string pageName, bool forceLoad = false);

    Task GoBackAsync();
}