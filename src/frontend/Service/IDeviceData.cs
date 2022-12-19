using System.Runtime.CompilerServices;
using WeeControl.Frontend.AppService.DeviceInterfaces;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("ApplicationService.UnitTest")]
namespace WeeControl.Frontend.AppService;

public interface IDeviceData : ICommunication, IFeature, IGui, IMedia, ISharing, IStorage
{
}