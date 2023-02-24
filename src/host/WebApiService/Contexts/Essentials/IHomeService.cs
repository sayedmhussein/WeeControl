using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IHomeService
{
    Task<bool> Refresh();
    Task<IEnumerable<HomeNotificationModel>> GetNotifications();
    Task<IEnumerable<HomeFeedModel>> GetFeeds();
    Task<string> GetFullName();
    Task MarkNotificationAsViewed(Guid id);
}