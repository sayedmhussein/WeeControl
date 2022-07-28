using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.Domain.Contexts.Essential;

public class TerritoryDbo
{
    public static TerritoryDbo Create(string code, string parent, string country, string name)
    {
        return new TerritoryDbo
        {
            TerritoryId = code,
            ReportToId = parent,
            CountryCode = country,
            TerritoryName = name
        };
    }

    public static TerritoryDbo Create(ITerritoryModel model)
    {
        return new TerritoryDbo()
        {
            TerritoryId = model.TerritoryCode,
            ReportToId = model.ReportToId,
            CountryCode = model.CountryCode,
            TerritoryName = model.TerritoryName,
            AlternativeName = model.LocalName
        };
    }
    
    [MinLength(3)] public string TerritoryId { get; set; } 
    public string TerritoryName { get; set; }
    public string AlternativeName { get; set; }
    [MinLength(3)] public string CountryCode { get; set; }
    
    public TerritoryDbo ReportTo { get; set; }
    public string ReportToId { get; set; }
    public ICollection<TerritoryDbo> Reporting { get; set; }
    

    private TerritoryDbo()
    {
    }
}