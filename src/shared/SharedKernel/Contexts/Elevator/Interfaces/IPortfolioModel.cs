namespace WeeControl.SharedKernel.Contexts.Elevator.Interfaces;

public interface IPortfolioModel : IFieldDataModel
{
    string CountryName { get; set; }
    string TerritoryName { get; set; }
    string SalesName { get; set; }
    
    string LastVisitDetails { get; set; }
}