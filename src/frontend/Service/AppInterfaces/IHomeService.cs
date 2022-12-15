using WeeControl.Frontend.AppService.AppModels;

namespace WeeControl.Frontend.AppService.AppInterfaces;

public interface IHomeService
{
    Task<bool> Sync();
    Task<string> GetGreetingMessage();
    
    Task<IEnumerable<HomeFeedModel>> GetHomeFeeds();
    Task<IEnumerable<HomeNotificationModel>> GetHomeNotifications();
}