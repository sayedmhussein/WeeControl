namespace WeeControl.SharedKernel.Essential.Interfaces;

public interface ITerritoryModel
{ 
    string TerritoryCode { get; }
    
    string ReportToId { get; }

    string CountryCode { get; }

    string TerritoryName { get; }
    
    string LocalName { get; }
}