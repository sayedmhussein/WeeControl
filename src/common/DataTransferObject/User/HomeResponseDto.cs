namespace WeeControl.Core.DataTransferObject.User;

public class HomeResponseDto
{
    public IEnumerable<INotificationModel> Notifications { get; set; }
    public IEnumerable<IHomeFeedModel> Feeds { get; set; }

}