using Microsoft.AspNetCore.Components.Forms;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IHomeService
{
    UserDataModel UserData { get; }
    IEnumerable<HomeNotificationModel> Notifications { get; }
    IEnumerable<HomeFeedModel> Feeds { get; }

    Task<bool> PullData();
    Task MarkNotificationAsViewed(Guid id);
    Task ChangePassword(UserPasswordChangeRequestDto dto);
    Task SendFeedback(string message, IEnumerable<IBrowserFile> files);
}