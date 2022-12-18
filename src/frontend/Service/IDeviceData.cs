using WeeControl.Frontend.AppService.DeviceInterfaces;

namespace WeeControl.Frontend.AppService;

public interface IDeviceData : ICommunication, IFeature, IGui, IMedia, ISharing, IStorage
{
}