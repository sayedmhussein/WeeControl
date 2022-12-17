namespace WeeControl.Common.SharedKernel.Contexts.Home;

public class HomeResponseDto
{
    public IEnumerable<INotificationModel> Notifications { get; set; }
    public IEnumerable<IHomeFeedModel> Feeds { get; set; }
    
}