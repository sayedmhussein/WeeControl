namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDevice
{
    IDeviceAlert Alert { get; }
    IDeviceLocation Location { get; }
    IDeviceSecurity Security { get; }
    IDeviceServerCommunication Server { get; }
    IDeviceStorage Storage { get; }
    IDevicePageNavigation Navigation { get; }
    
    public string DeviceId { get; }
    public DateTime CurrentTs { get; }
}