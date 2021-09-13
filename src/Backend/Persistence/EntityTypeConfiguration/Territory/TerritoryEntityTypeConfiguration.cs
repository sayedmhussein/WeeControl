using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.EntityGroups.Territory;

namespace WeeControl.Backend.Persistence.EntityTypeConfiguration.Territory
{
    public class TerritoryEntityTypeConfiguration : IEntityTypeConfiguration<TerritoryDbo>
    {
        public void Configure(EntityTypeBuilder<TerritoryDbo> builder)
        {
            builder.HasOne(e => e.ReportTo).WithMany();
            builder.ToTable("Territory".ToSnakeCase(), nameof(Territory).ToSnakeCase());
            builder.HasComment("Territory of corporate.");
            builder.HasIndex(x => new { x.CountryId, x.Name }).IsUnique(true);

            builder.HasMany(x => x.ReportingFrom).WithOne(x => x.ReportTo).HasForeignKey(x => x.ReportToId).OnDelete(DeleteBehavior.Restrict);

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
