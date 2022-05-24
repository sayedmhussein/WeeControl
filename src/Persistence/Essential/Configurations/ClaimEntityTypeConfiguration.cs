using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Domain.Essential.Entities;

namespace WeeControl.Persistence.Essential.Configurations
{
    public class ClaimEntityTypeConfiguration : IEntityTypeConfiguration<ClaimDbo>
    {
        public void Configure(EntityTypeBuilder<ClaimDbo> builder)
        {
            builder.ToTable(nameof(ClaimDbo), nameof(Essential));
            builder.Property(p => p.ClaimId).ValueGeneratedOnAdd();
            builder.Property(p => p.GrantedTs).HasDefaultValue(DateTime.UtcNow);
            builder.HasIndex(i => new { i.ClaimType, i.ClaimValue }).IsUnique();
        }
    }
}
