using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Backend.Domain.Databases.Essential.DatabaseObjects.EssentialsObjects;

public class TerritoryDbo
{
    [Key]
    public string TerritoryCode { get; set; }

    public TerritoryDbo ReportTo { get; set; }
    public string ReportToId { get; set; }

    public ICollection<TerritoryDbo> Reporting { get; set; }

    public string CountryCode { get; set; }

    public string TerritoryName { get; set; }

    public virtual IEnumerable<UserDbo> Users { get; set; }

}