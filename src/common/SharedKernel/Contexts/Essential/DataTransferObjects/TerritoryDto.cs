using WeeControl.Common.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.Common.SharedKernel.Contexts.Essential.DataTransferObjects;

public class TerritoryDto : TerritoryEntity
{
    public string? ReportToName { get; set; } = string.Empty;

    public TerritoryDto()
    {
    }

    public TerritoryDto(string? reportTo, TerritoryEntity territory)
    {
        ReportToName = reportTo;
        UniqueName = territory.UniqueName;
        AlternativeName = territory.AlternativeName;
        CountryCode = territory.CountryCode;
    }
}