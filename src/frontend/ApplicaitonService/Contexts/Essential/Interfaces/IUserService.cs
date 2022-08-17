using WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Contexts.Essential.Interfaces;

public interface IUserService : IViewModelBase
{
    IEnumerable<CountryModel> Countries { get; }
    string GreetingMessage { get; }
    bool IsEmployee { get; }
    bool IsCustomer { get; }
    List<MenuItemModel> MenuItems { get; }
    public List<HomeFeedModel> FeedsList { get; }
    public List<HomeNotificationModel> NotificationsList { get; }

    Task<bool> UsernameAllowed();
    Task<bool> EmailAllowed();
    Task<bool> MobileNumberAllowed();
    
    Task Init();
    Task Refresh();
    Task Register(CustomerRegisterModel registerModel);
    Task RequestPasswordReset(PasswordResetModel passwordResetModel);
    Task ChangeMyPassword(PasswordChangeModel passwordChangeModel);
}