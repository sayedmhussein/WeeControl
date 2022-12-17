namespace WeeControl.Frontend.AppService.GuiInterfaces.Obsolute;

[Obsolete("Use IGui instead.", true)]
public interface IDeviceAlert
{
    Task DisplayAlert(string message);
}