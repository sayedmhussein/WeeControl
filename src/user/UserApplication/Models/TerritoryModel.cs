using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.User.UserApplication.Models;

public class TerritoryModel : ITerritory
{
    public string TerritoryCode { get; }

    public string ReportToId { get; }

    public string CountryCode { get; }

    public string TerritoryName { get; }
    
    public string LocalName { get; }

    public TerritoryModel(ITerritory territory)
    {
        TerritoryCode = territory.TerritoryCode;
        ReportToId = territory.ReportToId;
        CountryCode = territory.CountryCode;
        TerritoryName = territory.TerritoryName;
        LocalName = territory.LocalName;
    }
}