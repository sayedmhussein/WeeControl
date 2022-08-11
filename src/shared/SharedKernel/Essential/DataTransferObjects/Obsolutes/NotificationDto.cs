using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class NotificationDto : IUserNotificationModel
{
    public Guid NotificationId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;

    public DateTime? ReadTs { get; set; }
}