using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Domain.Contexts.Essential.Configurations
{
    public class SessionLogEntityTypeConfiguration : IEntityTypeConfiguration<SessionLogDbo>
    {
        public void Configure(EntityTypeBuilder<SessionLogDbo> builder)
        {
            builder.ToTable(nameof(SessionLogDbo), nameof(Essential));
            builder.Property(p => p.LogId).ValueGeneratedOnAdd();
            builder.Property(p => p.LogTs).HasDefaultValue(DateTime.UtcNow);
            
            builder.HasOne(x => x.Session)
                .WithMany()
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
