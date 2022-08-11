using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.SharedKernel.Essential.Entities;

namespace WeeControl.Domain.Contexts.Essential;

[Table(nameof(TerritoryDbo), Schema = nameof(Essential))]
public class TerritoryDbo : TerritoryEntity
{
    [Key]
    public Guid TerritoryId { get; } = Guid.NewGuid();

    public Guid? ReportToId { get; set; }
    
    public TerritoryDbo ReportTo { get; }
    
    public ICollection<TerritoryDbo> ReportingTo { get; }

    private TerritoryDbo()
    {
    }

    public TerritoryDbo(string uniqueName, string alternativeName, string country, Guid? reportToId = null)
    {
        UniqueName = uniqueName.Trim().ToUpper();
        AlternativeName = alternativeName?.Trim();
        CountryCode = country.Trim().ToUpper();
        ReportToId = reportToId;
    }
}

public class TerritoryEntityTypeConfig : IEntityTypeConfiguration<TerritoryDbo>
{
    public void Configure(EntityTypeBuilder<TerritoryDbo> builder)
    {
        builder.Property(x => x.TerritoryId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());
        builder.HasComment("Territory of corporate.");
        builder.HasIndex(x => new { x.CountryCode, TerritoryName = x.UniqueName }).IsUnique();
        
        builder.HasOne(e => e.ReportTo).WithMany();
        builder.HasMany(x => x.ReportingTo)
            .WithOne(x => x.ReportTo)
            .HasForeignKey(x => x.ReportToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}