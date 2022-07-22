namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IDeviceServerCommunication
{
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}