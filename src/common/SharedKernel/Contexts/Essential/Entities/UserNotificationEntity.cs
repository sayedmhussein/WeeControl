namespace WeeControl.SharedKernel.Contexts.Essential.Entities;

public class UserNotificationEntity
{
    public Guid NotificationId { get; set; }
    
    public string Subject { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public DateTime PublishedTs { get; set; }
    public DateTime? ReadTs { get; set; }
}