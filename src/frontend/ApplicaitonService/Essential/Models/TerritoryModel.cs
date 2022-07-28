using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Essential.Models;

public class TerritoryModel : ITerritoryModel
{
    [Required]
    [StringLength(10, MinimumLength = 3)]
    public string TerritoryCode { get; set; }

    [StringLength(10, MinimumLength = 3)]
    public string ReportToId { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; }

    [Required]
    public string TerritoryName { get; set; }
    
    public string LocalName { get; set; }

    public TerritoryModel()
    {
    }

    public TerritoryModel(ITerritoryModel territoryModel)
    {
        TerritoryCode = territoryModel.TerritoryCode;
        ReportToId = territoryModel.ReportToId;
        CountryCode = territoryModel.CountryCode;
        TerritoryName = territoryModel.TerritoryName;
        LocalName = territoryModel.LocalName;
    }
}