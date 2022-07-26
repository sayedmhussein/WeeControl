using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Domain.Contexts.Essential.Configurations;

public class NotificationEntityTypeConfig : IEntityTypeConfiguration<NotificationDbo>
{
    public void Configure(EntityTypeBuilder<NotificationDbo> builder)
    {
        builder.ToTable(nameof(NotificationDbo), schema: nameof(Essential));
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.NotificationId).HasDefaultValue(Guid.NewGuid());

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.Subject).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Details).HasMaxLength(255);
        builder.Property(x => x.Link).HasMaxLength(255);

        builder.Property(x => x.PublishedTs).HasDefaultValue(DateTime.UtcNow);
    }
}