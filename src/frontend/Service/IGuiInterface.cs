using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.AppService;

public interface IGuiInterface : ICommunication, IFeature, IGui, IMedia, ISharing, IStorage
{
}