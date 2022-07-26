using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    
    [MinLength(3)] public string TerritoryId { get; set; }
    public string TerritoryName { get; set; }
    [MinLength(3)] public string CountryCode { get; set; }
    
    public TerritoryDbo ReportTo { get; set; }
    public string ReportToId { get; set; }
    public ICollection<TerritoryDbo> Reporting { get; set; }
    

    private TerritoryDbo()
    {
    }
}