using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.AppService.Interfaces;

public interface IDevice
{
    IDeviceAlert Alert { get; }
    IDeviceLocation Location { get; }
    IDeviceSecurity Security { get; }
    IDeviceServerCommunication Server { get; }
    IStorage Storage { get; }
    IDevicePageNavigation Navigation { get; }
    
    public string DeviceId { get; }
    public DateTime CurrentTs { get; }
}