using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Essential.Entities;

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
    
    [Key]
    [StringLength(10, MinimumLength = 3)]
    public string TerritoryId { get; set; }

    public TerritoryDbo ReportTo { get; set; }
    public string ReportToId { get; set; }

    public ICollection<TerritoryDbo> Reporting { get; set; }

    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; }

    [StringLength(20)]
    public string TerritoryName { get; set; }

    public virtual IEnumerable<UserDbo> Users { get; set; }

    private TerritoryDbo()
    {
    }
}