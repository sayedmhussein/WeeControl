using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.User.UserApplication.Models;

public class TerritoryModelModel : ITerritoryModel
{
    public string TerritoryCode { get; }

    public string ReportToId { get; }

    public string CountryCode { get; }

    public string TerritoryName { get; }
    
    public string LocalName { get; }

    public TerritoryModelModel(ITerritoryModel territoryModel)
    {
        TerritoryCode = territoryModel.TerritoryCode;
        ReportToId = territoryModel.ReportToId;
        CountryCode = territoryModel.CountryCode;
        TerritoryName = territoryModel.TerritoryName;
        LocalName = territoryModel.LocalName;
    }
}