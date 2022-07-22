namespace WeeControl.User.UserApplication.Interfaces;

public interface IDeviceAlert
{
    Task DisplayAlert(string message);
}