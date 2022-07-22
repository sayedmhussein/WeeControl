using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class TerritoryModelDto : ITerritoryModel
{
    public string TerritoryCode { get; set; } = string.Empty;
    
    public string ReportToId { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public string TerritoryName { get; set; } = string.Empty;

    public string LocalName { get; set; } = string.Empty;
}