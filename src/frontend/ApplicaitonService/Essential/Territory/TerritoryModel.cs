using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Essential.Territory;

public class TerritoryModel : ITerritoryModel
{
    public string TerritoryCode { get; }

    public string ReportToId { get; }

    public string CountryCode { get; }

    public string TerritoryName { get; }
    
    public string LocalName { get; }

    public TerritoryModel(ITerritoryModel territoryModel)
    {
        TerritoryCode = territoryModel.TerritoryCode;
        ReportToId = territoryModel.ReportToId;
        CountryCode = territoryModel.CountryCode;
        TerritoryName = territoryModel.TerritoryName;
        LocalName = territoryModel.LocalName;
    }
}