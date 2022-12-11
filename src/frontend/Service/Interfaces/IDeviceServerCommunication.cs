namespace WeeControl.Frontend.AppService.Interfaces;

public interface IDeviceServerCommunication
{
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}