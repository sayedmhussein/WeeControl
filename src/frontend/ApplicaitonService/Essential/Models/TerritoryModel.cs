using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Essential.Models;

public class TerritoryModel : ITerritoryModel
{
    public string TerritoryCode { get; set; }

    public string ReportToId { get; set; }

    public string CountryCode { get; set; }

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