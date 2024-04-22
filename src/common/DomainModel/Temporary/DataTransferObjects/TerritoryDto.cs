namespace WeeControl.Core.DomainModel.Temporary.DataTransferObjects;

public class TerritoryDto : TerritoryEntity
{
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

    public string? ReportToName { get; set; } = string.Empty;
}