using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DomainModel.Essentials;

public class HomeNotificationModel : IEntityModel
{
    public Guid NotificationId { get; set; }

    [Required] [StringLength(25)] public string Subject { get; set; } = string.Empty;

    [AllowNull] [StringLength(255)] public string Body { get; set; } = string.Empty;

    [AllowNull] [StringLength(255)] public string NotificationUrl { get; set; } = string.Empty;

    public DateTime PublishedTs { get; set; } = DateTime.UtcNow;

    public DateTime? ReadTs { get; set; }
}