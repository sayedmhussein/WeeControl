using System;
using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.Domain.Contexts.Essential;

public class NotificationDbo : IUserNotificationModel
{
    public static NotificationDbo Create(Guid userid, string subject, string details, string link)
    {
        return new NotificationDbo()
        {
            UserId = userid,
            Subject = subject,
            Details = details,
            Link = link
        };
    }
    
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public DateTime PublishedTs { get; set; }
    public DateTime? ReadTs { get; set; }
}