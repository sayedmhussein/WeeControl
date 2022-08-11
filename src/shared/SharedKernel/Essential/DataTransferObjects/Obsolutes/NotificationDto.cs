

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class NotificationDto
{
    public Guid NotificationId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;

    public DateTime? ReadTs { get; set; }
}