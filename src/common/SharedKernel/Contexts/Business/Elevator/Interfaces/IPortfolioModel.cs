namespace WeeControl.Common.SharedKernel.Contexts.Business.Elevator.Interfaces;

public interface IPortfolioModel : IFieldDataModel
{
    string CountryName { get; set; }
    string TerritoryName { get; set; }
    string SalesName { get; set; }
    
    string LastVisitDetails { get; set; }
}