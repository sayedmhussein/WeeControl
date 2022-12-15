namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

public interface IDeviceServerCommunication
{
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}