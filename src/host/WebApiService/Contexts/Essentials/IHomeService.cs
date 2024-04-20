using Microsoft.AspNetCore.Components.Forms;
using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.DomainModel.Essentials.Dto;

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