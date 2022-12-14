namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

public interface IDeviceAlert
{
    Task DisplayAlert(string message);
}