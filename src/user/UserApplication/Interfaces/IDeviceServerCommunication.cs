namespace WeeControl.User.UserApplication.Interfaces;

public interface IDeviceServerCommunication
{
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}