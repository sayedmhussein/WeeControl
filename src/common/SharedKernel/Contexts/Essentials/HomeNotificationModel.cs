using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.Essentials;

public class HomeNotificationModel : IEntityModel
{
    public Guid NotificationId { get; set; }

    [Required]
    [StringLength(25)]
    public string Subject { get; set; } = string.Empty;

    [AllowNull]
    [StringLength(255)]
    public string Body { get; set; } = string.Empty;
    
    [AllowNull]
    [StringLength(255)]
    public string NotificationUrl { get; set; } = string.Empty;

    public DateTime PublishedTs { get; set; }
    
    public DateTime? ReadTs { get; set; }
}