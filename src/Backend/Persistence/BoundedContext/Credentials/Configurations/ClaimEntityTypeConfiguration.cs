using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.Databases.Essential.DatabaseObjects.EssentialsObjects;

namespace WeeControl.Backend.Persistence.BoundedContext.Credentials.Configurations
{
    public class ClaimEntityTypeConfiguration : IEntityTypeConfiguration<ClaimDbo>
    {
        public void Configure(EntityTypeBuilder<ClaimDbo> builder)
        {
            builder.ToTable("claim", "credentials");
            builder.Property(p => p.ClaimId).ValueGeneratedOnAdd();
            builder.Property(p => p.GrantedTs).HasDefaultValue(DateTime.UtcNow);
        }
    }
}
