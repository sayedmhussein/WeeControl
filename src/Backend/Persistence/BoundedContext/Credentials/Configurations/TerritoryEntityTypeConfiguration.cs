using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.Databases.Databases.DatabaseObjects;
using WeeControl.Backend.Domain.Databases.Databases.DatabaseObjects.EssentialsObjects;

namespace WeeControl.Backend.Persistence.BoundedContext.Credentials.Configurations
{
    public class TerritoryEntityTypeConfiguration : IEntityTypeConfiguration<TerritoryDbo>
    {
        public void Configure(EntityTypeBuilder<TerritoryDbo> builder)
        {
            builder.ToTable("territory", "credentials");
            builder.HasComment("Territory of corporate.");
            
            builder.Property(p => p.TerritoryCode).ValueGeneratedOnAdd();
            
            builder.HasIndex(x => new { x.CountryCode, x.TerritoryName }).IsUnique(true);
            
            builder.HasOne(e => e.ReportTo).WithMany();
            builder.HasMany(x => x.Reporting).WithOne(x => x.ReportTo).HasForeignKey(x => x.ReportToId).OnDelete(DeleteBehavior.Restrict);

            //builder.OwnsOne(x => x.Address);

            builder.HasMany(x => x.Users).WithOne().HasForeignKey(x => x.TerritoryCode);
        }
    }
}