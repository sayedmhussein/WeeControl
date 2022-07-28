using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Domain.Contexts.Essential.Configurations
{
    public class TerritoryEntityTypeConfiguration : IEntityTypeConfiguration<TerritoryDbo>
    {
        public void Configure(EntityTypeBuilder<TerritoryDbo> builder)
        {
            builder.ToTable(nameof(TerritoryDbo), nameof(Essential));
            builder.HasKey(x => x.TerritoryId);
            builder.Property(x => x.TerritoryId).HasMaxLength(10);
            builder.HasOne(e => e.ReportTo).WithMany();
            builder.HasMany(x => x.Reporting)
                .WithOne(x => x.ReportTo)
                .HasForeignKey(x => x.ReportToId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasComment("Territory of corporate.");

            builder.Property(x => x.CountryCode).HasMaxLength(3).IsRequired();
            builder.Property(x => x.TerritoryName).HasMaxLength(50).IsRequired();
            builder.HasIndex(x => new { x.CountryCode, x.TerritoryName }).IsUnique();
            
            builder.Property(x => x.AlternativeName).HasMaxLength(50);

            //builder.OwnsOne(x => x.Address);

            //builder.HasMany(x => x.Users).WithOne().HasForeignKey(x => x.TerritoryId);
        }
    }
}