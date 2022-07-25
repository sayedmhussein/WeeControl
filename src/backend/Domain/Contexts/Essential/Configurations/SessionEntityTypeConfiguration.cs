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
            builder.Property(p => p.SessionId).ValueGeneratedOnAdd();
            builder.Property(p => p.CreatedTs).HasDefaultValue(DateTime.UtcNow);
        }
    }
}
