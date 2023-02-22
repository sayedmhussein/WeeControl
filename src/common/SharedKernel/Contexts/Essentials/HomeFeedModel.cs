using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.Essentials;

public class HomeFeedModel : IEntityModel
{
    [Required] 
    [StringLength(25)]
    public string FeedSubject { get; init; } = string.Empty;
    
    [StringLength(25)]
    public string FeedBody { get; init; } = string.Empty;

    [StringLength(255)]
    public string FeedUrl { get; init; } = string.Empty;

    public DateTime FeedTs { get; init; }
}