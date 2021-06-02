using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.EntityDbo.PublicSchema;

namespace MySystem.Persistence.EntityTypeConfiguration.PublicSchema
{
    public class TerritoryEntityTypeConfiguration : IEntityTypeConfiguration<TerritoryDbo>
    {
        public void Configure(EntityTypeBuilder<TerritoryDbo> builder)
        {
            builder.HasOne(e => e.ReportTo).WithMany();
            builder.ToTable("Office", "Basic");
            builder.HasComment("Offices of corporate.");
            builder.HasIndex(x => new { x.CountryId, x.OfficeName }).IsUnique(true);

            builder.Property(x => x.ReportToId).HasComment("Local inhertance from this table primay key.");

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
