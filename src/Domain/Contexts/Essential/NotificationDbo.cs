using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Domain.Contexts.Essential;

[Table(nameof(NotificationDbo), Schema = nameof(Essential))]
public class NotificationDbo
{
    [Key]
    public Guid NotificationId { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;
    
    public DateTime? ViewedTs { get; set; }
}