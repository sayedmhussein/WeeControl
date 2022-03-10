using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities;

namespace WeeControl.Backend.Persistence.BoundedContexts.HumanResources.Configurations
{
    public class TerritoryEntityTypeConfiguration : IEntityTypeConfiguration<Territory>
    {
        public void Configure(EntityTypeBuilder<Territory> builder)
        {
            builder.ToTable(nameof(Territory), nameof(Territory));
            builder.HasComment("Territory of corporate.");
            
            builder.Property(p => p.TerritoryCode).ValueGeneratedOnAdd();
            
            builder.HasIndex(x => new { x.CountryCode, x.TerritoryName }).IsUnique(true);
            
            builder.HasOne(e => e.ReportTo).WithMany();
            builder.HasMany(x => x.Reporting).WithOne(x => x.ReportTo).HasForeignKey(x => x.ReportToId).OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(x => x.Address);
        }
    }
}