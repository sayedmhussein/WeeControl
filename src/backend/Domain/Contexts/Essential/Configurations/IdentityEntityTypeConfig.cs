using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Domain.Contexts.Essential.Configurations;

public class IdentityEntityTypeConfig : IEntityTypeConfiguration<IdentityDbo>
{
    public void Configure(EntityTypeBuilder<IdentityDbo> builder)
    {
        builder.ToTable(nameof(IdentityDbo), schema: nameof(Essential));
        builder.HasKey(x => x.IdentityId);
        builder.Property(x => x.IdentityId).ValueGeneratedOnAdd();

        builder.Property(x => x.Type).HasMaxLength(25).IsRequired();
        builder.Property(x => x.Number).HasMaxLength(25).IsRequired();
        builder.Property(x => x.CountryId).HasMaxLength(3).IsRequired();
    }
}