using WeeControl.Frontend.AppService.GuiInterfaces;

namespace WeeControl.Frontend.AppService;

public interface IDeviceData : ICommunication, IFeature, IGui, IMedia, ISharing, IStorage
{
}