using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Frontend.AppService.Models;

namespace WeeControl.Frontend.AppService.GuiInterfaces.Home;

public interface IHomeService
{
    IEnumerable<CountryModel> Countries { get; }
    List<MenuItemModel> MenuItems { get; }
    string GreetingMessage { get; }

    Task<bool> VerifyAuthentication();
    Task<bool> Sync();
    Task<IEnumerable<HomeFeedModel>> GetHomeFeeds();
    Task<IEnumerable<HomeNotificationModel>> GetHomeNotifications();

    Task Register(CustomerRegisterDto registerModel);
    Task RequestPasswordReset(UserPasswordResetRequestDto passwordResetModel);
    Task ChangeMyPassword(UserPasswordChangeRequestDto passwordChangeModel);

    public string FeedsLabel { get; }
    public string SyncButtonLabel { get; }
    public string InternetIssueMessage { get; }
}