using WeeControl.Common.SharedKernel.Contexts.Temporary.User;
using WeeControl.Frontend.AppService.Models;

namespace WeeControl.Frontend.AppService.GuiInterfaces.Home;

public interface IHomeService
{
    IEnumerable<CountryModel> Countries { get; }
    Task<bool> Sync();
    Task<string> GetGreetingMessage();
    List<MenuItemModel> MenuItems { get; }
    Task<IEnumerable<HomeFeedModel>> GetHomeFeeds();
    Task<IEnumerable<HomeNotificationModel>> GetHomeNotifications();
    
    Task Register(RegisterCustomerDto registerModel);
    Task RequestPasswordReset(UserPasswordResetRequestDto passwordResetModel);
    Task ChangeMyPassword(UserPasswordChangeRequestDto passwordChangeModel);

    public string FeedsLabel { get; }
    public string SyncButtonLabel { get; }
    public string InternetIssueMessage { get; }
}