namespace WeeControl.Core.DataTransferObject.User;

public interface IHomeFeedModel
{
    Guid FeedId { get; }
    string FeedString { get; }
    string FeedUrl { get; }
    DateTime FeedTs { get; }
}