namespace WeeControl.SharedKernel.Essential.Interfaces;

public interface ITerritory
{ 
    string TerritoryCode { get; }
    
    string ReportToId { get; }

    string CountryCode { get; }

    string TerritoryName { get; }
    
    string LocalName { get; }
}