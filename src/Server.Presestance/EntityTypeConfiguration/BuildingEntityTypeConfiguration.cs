using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Domain.EntityDbo;

namespace MySystem.Persistence.EntityTypeConfiguration
{
    public class BuildingEntityTypeConfiguration : IEntityTypeConfiguration<BuildingDbo>
    {
        public void Configure(EntityTypeBuilder<BuildingDbo> builder)
        {
            builder.ToTable("Building", "Basic");
            builder.HasComment("Building of corporate.");
            builder.HasIndex(x => new { x.CountryId, x.BuildingName }).IsUnique(true);

            if (MySystemDbContext.DbFacade.IsNpgsql())
            {
                builder.Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
            }
        }
    }
}
