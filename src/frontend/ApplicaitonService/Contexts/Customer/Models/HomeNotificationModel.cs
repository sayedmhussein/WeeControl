namespace WeeControl.Frontend.ApplicationService.Contexts.Customer.Models;

public class HomeNotificationModel
{
    public Guid NotificationId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string NotificationUrl { get; set; } = string.Empty;

    public DateTime NotificationTs { get; set; }
    
    public bool IsRead { get; set; }
}