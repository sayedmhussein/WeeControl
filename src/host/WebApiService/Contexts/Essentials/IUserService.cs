using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IUserService
{
    Task<bool> Refresh();
    Task<IEnumerable<HomeNotificationModel>> GetNotifications();
    Task<IEnumerable<HomeFeedModel>> GetFeeds();
    Task<string> GetFullName();
}