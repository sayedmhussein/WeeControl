namespace WeeControl.Frontend.AppService.Interfaces;

public interface IDeviceAlert
{
    Task DisplayAlert(string message);
}