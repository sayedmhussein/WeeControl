using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Exceptions;
using WeeControl.Core.SharedKernel.ExtensionMethods;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table(nameof(UserNotificationDbo), Schema = nameof(Essentials))]
public class UserNotificationDbo : HomeNotificationModel
{
    private UserNotificationDbo()
    {
    }

    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    public static UserNotificationDbo Create(Guid userid, string subject, string details, string link)
    {
        if (userid == Guid.Empty)
            throw new EntityDomainValidationException("User ID must not be empty GUID");

        var notification = new UserNotificationDbo
        {
            UserId = userid,
            Subject = subject,
            Body = details,
            NotificationUrl = link
        };

        notification.ThrowExceptionIfEntityModelNotValid();
        return notification;
    }
}

public class UserNotificationEntityTypeConfig : IEntityTypeConfiguration<UserNotificationDbo>
{
    public void Configure(EntityTypeBuilder<UserNotificationDbo> builder)
    {
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.NotificationId).ValueGeneratedOnAdd(); //.HasDefaultValue(Guid.NewGuid());

        builder.HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.PublishedTs).ValueGeneratedOnAdd();
    }
}