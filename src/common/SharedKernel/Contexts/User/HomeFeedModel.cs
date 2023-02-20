using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class HomeFeedModel : IEntityModel
{
    [StringLength(25)]
    public string FeedString { get; init; } = string.Empty;

    [StringLength(255)]
    public string FeedUrl { get; init; } = string.Empty;

    public DateTime FeedTs { get; init; }
}