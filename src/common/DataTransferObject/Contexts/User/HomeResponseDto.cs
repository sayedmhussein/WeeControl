namespace WeeControl.Core.DataTransferObject.Contexts.User;

public class HomeResponseDto
{
    public IEnumerable<INotificationModel> Notifications { get; set; }
    public IEnumerable<IHomeFeedModel> Feeds { get; set; }

}