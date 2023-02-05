using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.Business.Elevator.Configurations;

public class BuildingEntityTypeConfig : IEntityTypeConfiguration<BuildingDbo>
{
    public void Configure(EntityTypeBuilder<BuildingDbo> builder)
    {
        builder.ToTable(nameof(BuildingDbo), schema: nameof(Elevator));
        builder.HasKey(x => x.BuildingId);
        builder.Property(x => x.BuildingId).ValueGeneratedOnAdd();

        builder.Property(x => x.CountryId).HasMaxLength(3).IsRequired();
        builder.Property(x => x.BuildingName).HasMaxLength(45).IsRequired();
        builder.HasIndex(nameof(BuildingDbo.CountryId), nameof(BuildingDbo.BuildingName)).IsUnique();


        builder.HasMany(x => x.Units)
            .WithOne(x => x.Building)
            .HasForeignKey(x => x.BuildingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}