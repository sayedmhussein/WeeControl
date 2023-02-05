namespace WeeControl.Core.DataTransferObject.Contexts.User;

public interface IHomeFeedModel
{
    Guid FeedId { get; }
    string FeedString { get; }
    string FeedUrl { get; }
    DateTime FeedTs { get; }
}