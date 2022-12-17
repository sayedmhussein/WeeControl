using WeeControl.Common.SharedKernel.Contexts.Home;

namespace WeeControl.Frontend.AppService.Contexts.Home;

public class HomeFeedModel : IHomeFeedModel
{
    public Guid FeedId { get; }

    public string FeedString { get; } = string.Empty;

    public string FeedUrl { get; } = string.Empty;

    public DateTime FeedTs { get; }
}