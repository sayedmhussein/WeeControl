using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class TerritoryDto : ITerritoryModel
{
    public static TerritoryDto Create(ITerritoryModel model)
    {
        return new TerritoryDto()
        {
            TerritoryCode = model.TerritoryCode,
            TerritoryName = model.TerritoryName,
            CountryCode = model.CountryCode,
            ReportToId = model.ReportToId,
            LocalName = model.LocalName
        };
    }
    
    public string TerritoryCode { get; set; } = string.Empty;
    
    public string ReportToId { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public string TerritoryName { get; set; } = string.Empty;

    public string LocalName { get; set; } = string.Empty;
}