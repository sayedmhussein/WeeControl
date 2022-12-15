using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

namespace WeeControl.Frontend.MauiApp.Services;

public class ServiceCommunication : IDeviceServerCommunication
{
    public HttpClient HttpClient { get; }

    public ServiceCommunication()
    {
        HttpClient = new HttpClient();
    }
    
    public string GetFullAddress(string relative)
    {
        //return "http://192.168.45.107:5001/" + relative;
        return "https://localhost:5001/" + relative;
    }
}