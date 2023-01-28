namespace WeeControl.Core.DataTransferObject.User;

public interface INotificationModel
{
    Guid NotificationId { get; set; }

    string Subject { get; set; }

    string Body { get; set; }

    string NotificationUrl { get; set; }

    DateTime NotificationTs { get; set; }

    bool IsRead { get; set; }
}