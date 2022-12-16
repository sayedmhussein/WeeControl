namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

[Obsolete("HttpClient should be created on the service layer, then GenFullAddress should have own class.", true)]
public interface IDeviceServerCommunication
{
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}