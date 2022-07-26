using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Domain.Contexts.Essential.Configurations
{
    public class SessionEntityTypeConfiguration : IEntityTypeConfiguration<SessionDbo>
    {
        public void Configure(EntityTypeBuilder<SessionDbo> builder)
        {
            builder.ToTable(nameof(SessionDbo), nameof(Essential));
            builder.HasKey(x => x.SessionId);
            builder.Property(p => p.SessionId).ValueGeneratedOnAdd();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.DeviceId).HasMaxLength(255).IsRequired();
            
            builder.Property(p => p.CreatedTs).HasDefaultValue(DateTime.UtcNow);
            
            builder.HasMany(x => x.Logs)
                .WithOne().HasForeignKey(x => x.SessionId);
        }
    }
}
