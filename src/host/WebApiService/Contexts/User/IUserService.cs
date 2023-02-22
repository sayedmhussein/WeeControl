using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Host.WebApiService.Contexts.User;

public interface IUserService
{
    Task<bool> Refresh();
    Task<IEnumerable<HomeNotificationModel>> GetNotifications();
    Task<string> GetFullName();
}