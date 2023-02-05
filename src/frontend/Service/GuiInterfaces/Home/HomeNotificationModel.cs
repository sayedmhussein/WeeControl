using WeeControl.Core.DataTransferObject.Contexts.User;

namespace WeeControl.Frontend.AppService.GuiInterfaces.Home;

public class HomeNotificationModel : INotificationModel
{
    public Guid NotificationId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string NotificationUrl { get; set; } = string.Empty;

    public DateTime NotificationTs { get; set; }

    public bool IsRead { get; set; }
}