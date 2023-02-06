using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.User;

[Table(nameof(UserNotificationDbo), Schema = nameof(User))]
public class UserNotificationDbo
{
    public static UserNotificationDbo Create(Guid userid, string subject, string details, string link)
    {
        return new UserNotificationDbo()
        {
            UserId = userid,
            Subject = subject,
            Details = details,
            Link = link
        };
    }


    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    public Guid NotificationId { get; set; }

    public string Subject { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public DateTime PublishedTs { get; set; }
    public DateTime? ReadTs { get; set; }

}

public class UserNotificationEntityTypeConfig : IEntityTypeConfiguration<UserNotificationDbo>
{
    public void Configure(EntityTypeBuilder<UserNotificationDbo> builder)
    {
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.NotificationId).HasDefaultValue(Guid.NewGuid());

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
        
        builder.Property(x => x.PublishedTs).HasDefaultValue(DateTime.UtcNow);
    }
}