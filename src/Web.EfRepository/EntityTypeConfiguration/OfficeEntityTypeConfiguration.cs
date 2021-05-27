using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Shared.Library.Dbo.Entity;

namespace MySystem.Web.Infrastructure.EfRepository.EntityTypeConfiguration
{
    public class OfficeEntityTypeConfiguration : IEntityTypeConfiguration<OfficeDbo>
    {
        public void Configure(EntityTypeBuilder<OfficeDbo> builder)
        {
            builder.HasOne(e => e.Parent).WithMany();
            builder.ToTable("Office", "Basic");
            builder.HasComment("Offices of corporate.");
            builder.HasIndex(x => new { x.CountryId, x.OfficeName }).IsUnique(true);

            builder.Property(x => x.ParentId).HasComment("Local inhertance from this table primay key.");

            if (DataContext.DbFacade.IsNpgsql())
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
