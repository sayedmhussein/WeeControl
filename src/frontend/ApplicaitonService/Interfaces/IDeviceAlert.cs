namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IDeviceAlert
{
    Task DisplayAlert(string message);
}