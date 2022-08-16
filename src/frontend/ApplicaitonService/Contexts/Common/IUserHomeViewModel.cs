using WeeControl.Frontend.ApplicationService.Contexts.Customer.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Contexts.Common;

public interface IUserHomeViewModel : IViewModelBase
{
    string GreetingMessage { get; }
    bool IsEmployee { get; }
    bool IsCustomer { get; }
    List<MenuItemModel> MenuItems { get; }
    public List<HomeFeedModel> FeedsList { get; }
    public List<HomeNotificationModel> NotificationsList { get; }
    
    Task Init();
    Task Refresh();
    Task ChangeMyPasswordAsync();
}