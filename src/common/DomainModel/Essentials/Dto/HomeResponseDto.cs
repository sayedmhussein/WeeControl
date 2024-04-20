namespace WeeControl.Core.DomainModel.Essentials.Dto;

public class HomeResponseDto
{
    public string FullName { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;

    public IEnumerable<HomeNotificationModel> Notifications { get; set; } = new List<HomeNotificationModel>();
    public IEnumerable<HomeFeedModel> Feeds { get; set; } = new List<HomeFeedModel>();
}