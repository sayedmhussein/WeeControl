using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Server.Domain.Administration.Entities;

namespace WeeControl.Server.Persistence.Administration.Configutations
{
    public class TerritoryEntityTypeConfiguration : IEntityTypeConfiguration<Territory>
    {
        public void Configure(EntityTypeBuilder<Territory> builder)
        {
            builder.ToTable(nameof(Territory), nameof(Territory));
            builder.HasComment("Territory of corporate.");
            
            builder.Property(p => p.TerritoryCode).ValueGeneratedOnAdd();
            
            builder.HasIndex(x => new { x.CountryCode, TerritoryName = x.Name }).IsUnique(true);
            
            builder.HasOne(e => e.ReportTo).WithMany();
            builder.HasMany(x => x.Reporting).WithOne(x => x.ReportTo).HasForeignKey(x => x.ReportToId).OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(x => x.Address);
        }
    }
}