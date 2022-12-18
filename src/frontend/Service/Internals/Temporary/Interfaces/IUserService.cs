using WeeControl.Frontend.AppService.GuiInterfaces.Home;
using WeeControl.Frontend.AppService.Internals.Temporary.Models;

namespace WeeControl.Frontend.AppService.Internals.Temporary.Interfaces;

public interface IUserService
{
    IEnumerable<CountryModel> Countries { get; }
    string GreetingMessage { get; }
    bool IsEmployee { get; }
    bool IsCustomer { get; }
    List<MenuItemModel> MenuItems { get; }
    public List<HomeFeedModel> FeedsList { get; }
    public List<HomeNotificationModel> NotificationsList { get; }

    Task<bool> PropertyIsAllowed(string propertyName, string username);

    Task Init();
    Task Refresh();
    Task Register(CustomerRegisterModel registerModel);
    Task RequestPasswordReset(PasswordResetModel passwordResetModel);
    Task ChangeMyPassword(PasswordChangeModel passwordChangeModel);
}