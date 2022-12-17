using WeeControl.Frontend.AppService.GuiInterfaces;

namespace WeeControl.Frontend.AppService;

public interface IGuiInterface : ICommunication, IFeature, IGui, IMedia, ISharing, IStorage
{
}