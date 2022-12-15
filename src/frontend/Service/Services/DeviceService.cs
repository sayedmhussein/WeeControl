using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.AppService.Services;

public class DeviceService : IDevice
{
    public DeviceService(IDeviceAlert alert, IDeviceLocation location, IDeviceSecurity security, IDeviceServerCommunication server, IStorage storage, IDevicePageNavigation navigation)
    {
        Alert = alert;
        Location = location;
        Security = security;
        Server = server;
        Storage = storage;
        Navigation = navigation;
    }

    public IDeviceAlert Alert { get; }
    public IDeviceLocation Location { get; }
    public IDeviceSecurity Security { get; }
    public IDeviceServerCommunication Server { get; }
    public IStorage Storage { get; }
    public IDevicePageNavigation Navigation { get; }
    public string DeviceId => "__Blazor__";
    public DateTime CurrentTs => DateTime.Now;
}