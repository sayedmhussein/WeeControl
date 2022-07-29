namespace WeeControl.SharedKernel.Essential.Interfaces;

public interface IUserNotificationModel
{
    Guid NotificationId { get; set; }

    string Subject { get; set; }

    string Details { get; set; }

    string Link { get; set; }
    
    DateTime? ReadTs { get; set; }
}