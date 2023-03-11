using Microsoft.AspNetCore.Components.Forms;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IHomeService
{
    IEnumerable<HomeNotificationModel> Notifications { get; }
    IEnumerable<HomeFeedModel> Feeds { get; }
    string Fullname { get; }

    Task<bool> Refresh();
    Task MarkNotificationAsViewed(Guid id);

    Task SendFeedback(string message, IEnumerable<IBrowserFile> files);
}