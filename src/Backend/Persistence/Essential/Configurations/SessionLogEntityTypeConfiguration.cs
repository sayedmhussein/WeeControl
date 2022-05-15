using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Domain.Essential.Entities;

namespace WeeControl.Persistence.Essential.Configurations
{
    public class SessionLogEntityTypeConfiguration : IEntityTypeConfiguration<SessionLogDbo>
    {
        public void Configure(EntityTypeBuilder<SessionLogDbo> builder)
        {
            builder.ToTable(nameof(SessionLogDbo), nameof(Essential));
            builder.Property(p => p.LogId).ValueGeneratedOnAdd();
            builder.Property(p => p.LogTs).HasDefaultValue(DateTime.UtcNow);
        }
    }
}
