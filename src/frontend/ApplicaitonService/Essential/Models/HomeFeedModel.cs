namespace WeeControl.Frontend.ApplicationService.Essential.Models;

public class HomeFeedModel
{
    public Guid FeedId { get; }

    public string FeedString { get; } = string.Empty;

    public string FeedUrl { get; } = string.Empty;

    public DateTime FeedTs { get; }
}