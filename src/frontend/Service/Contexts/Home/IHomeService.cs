namespace WeeControl.Frontend.AppService.Contexts.Home;

public interface IHomeService
{
    Task<bool> Sync();
    Task<string> GetGreetingMessage();
    
    Task<IEnumerable<HomeFeedModel>> GetHomeFeeds();
    Task<IEnumerable<HomeNotificationModel>> GetHomeNotifications();
}