namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

[Obsolete("Use IDevice instead.")]
public interface IEssentialDeviceServerDevice : IUserDevice, IDeviceServerCommunication, IDeviceStorage
{
}