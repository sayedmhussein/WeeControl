using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.SharedKernel.Essential.Entities;
using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.Domain.Contexts.Essential;

[Table(nameof(TerritoryDbo), Schema = nameof(Essential))]
public class TerritoryDbo : TerritoryEntity
{
    [Obsolete]
    public static TerritoryDbo Create(string code, string parent, string country, string name)
    {
        return new TerritoryDbo
        {
            TerritoryIdObsolute = code,
            ReportToIdObsolute = parent,
            CountryCode = country,
            UniqueName = name
        };
    }

    [Obsolete]
    public static TerritoryDbo Create(ITerritoryModel model)
    {
        return new TerritoryDbo()
        {
            TerritoryIdObsolute = model.TerritoryCode,
            ReportToIdObsolute = model.ReportToId,
            CountryCode = model.CountryCode,
            UniqueName = model.TerritoryName,
            AlternativeName = model.LocalName
        };
    }

    [Key]
    public Guid TerritoryId { get; set; }

    public Guid? ReportToId { get; set; }
    
    public TerritoryDbo ReportTo { get; set; }
    
    public ICollection<TerritoryDbo> ReportingTo { get; set; }
    
    [Obsolete]
    [MinLength(3)] 
    public string TerritoryIdObsolute { get; set; } 
    
    [Obsolete]
    public string ReportToIdObsolute { get; set; }
    [Obsolete]
    public ICollection<TerritoryDbo> Reporting { get; set; }
    

    private TerritoryDbo()
    {
    }
}

public class TerritoryEntityTypeConfig : IEntityTypeConfiguration<TerritoryDbo>
{
    public void Configure(EntityTypeBuilder<TerritoryDbo> builder)
    {
        builder.HasComment("Territory of corporate.");
        builder.HasIndex(x => new { x.CountryCode, TerritoryName = x.UniqueName }).IsUnique();
        builder.HasOne(e => e.ReportTo).WithMany();
        builder.HasMany(x => x.ReportingTo)
            .WithOne(x => x.ReportTo)
            .HasForeignKey(x => x.ReportToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}