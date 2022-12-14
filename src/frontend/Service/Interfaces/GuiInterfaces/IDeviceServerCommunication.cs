namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

public interface IDeviceServerCommunication
{
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}