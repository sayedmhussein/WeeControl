namespace WeeControl.Common.SharedKernel.Contexts.Home;

public interface IHomeFeedModel
{ 
    Guid FeedId { get; }
    string FeedString { get; }
    string FeedUrl { get; }
    DateTime FeedTs { get; }
}