namespace WeeControl.Frontend.Service.Interfaces;

public interface IDeviceServerCommunication
{
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}