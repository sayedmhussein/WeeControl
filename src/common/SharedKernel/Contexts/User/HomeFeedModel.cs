using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class HomeFeedModel
{
    [StringLength(25)]
    public string FeedString { get; init; } = string.Empty;

    [StringLength(255)]
    public string FeedUrl { get; init; } = string.Empty;

    public DateTime FeedTs { get; init; }
}