using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.BoundedContexts.Credentials.DatabaseObjects;

namespace WeeControl.Backend.Persistence.Credentials.Configurations
{
    public class ClaimEntityTypeConfiguration : IEntityTypeConfiguration<ClaimDbo>
    {
        public void Configure(EntityTypeBuilder<ClaimDbo> builder)
        {
            builder.ToTable("claim", "credentials");
            builder.Property(p => p.ClaimId).ValueGeneratedOnAdd();
        }
    }
}
