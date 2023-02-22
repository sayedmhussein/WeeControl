using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.Business;

[Table(nameof(TerritoryDbo), Schema = nameof(Business))]
public class TerritoryDbo
{
    [Key]
    public Guid TerritoryId { get; } = Guid.NewGuid();

    public Guid? ReportToId { get; set; }

    public TerritoryDbo ReportTo { get; }

    public ICollection<TerritoryDbo> ReportingTo { get; }

    [Required]
    [StringLength(255, MinimumLength = 3)]
    public string UniqueName { get; set; } = string.Empty;

    [StringLength(255)]
    public string AlternativeName { get; set; } = string.Empty;

    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; } = string.Empty;

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

    public TerritoryDbo(string uniqueName, Guid? reportToId, string alternativeName, string country)
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
        builder.HasIndex(x => new { x.CountryCode, TerritoryName = x.UniqueName }).IsUnique();

        builder.HasOne(e => e.ReportTo).WithMany();
        builder.HasMany(x => x.ReportingTo)
            .WithOne(x => x.ReportTo)
            .HasForeignKey(x => x.ReportToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}