namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

public interface IDevice
{
    IDeviceAlert DeviceAlert { get; }
    IDeviceLocation DeviceLocation { get; }
    IDeviceSecurity DeviceSecurity { get; }
    IDeviceServerCommunication DeviceServerCommunication { get; }
    IDeviceStorage DeviceStorage { get; }
    IDevicePageNavigation DevicePageNavigation { get; }
    
    public string DeviceId { get; }
    public DateTime CurrentTs { get; }
}