namespace WeeControl.Frontend.Service.Interfaces;

public interface IDeviceAlert
{
    Task DisplayAlert(string message);
}