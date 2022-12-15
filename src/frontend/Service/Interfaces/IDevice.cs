using System.Reflection;
using System.Runtime.CompilerServices;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;
//[assembly: InternalsVisibleTo("UserApplication.Test.Integration")]

namespace WeeControl.Frontend.AppService.Interfaces;

internal interface IDevice
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