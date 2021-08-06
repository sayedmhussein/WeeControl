namespace WeeControl.Applications.BaseLib.Interfaces
{
    /// <summary>
    /// To be implemented on each device, then registered as singleton.
    /// </summary>
    public interface IDevice : IDeviceCommunication, IDeviceStorage, IDeviceState, IDeviceAction
    {
    }
}
