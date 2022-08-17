using WeeControl.SharedKernel.Contexts.Business.Elevator.Enums;
using WeeControl.SharedKernel.Contexts.Business.Elevator.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Contexts.Business.Elevator;

public class PortfolioModel : IPortfolioModel
{
    public string ContractNo { get; set; }
    public ContractTypeEnum ContractType { get; set; }
    public ContractStateEnum ContractState { get; set; }
    public string UnitNo { get; set; }
    public UnitTypeEnum UnitType { get; set; }
    public UnitStateTypeEnum UnitState { get; set; }
    public string BuildingName { get; set; }
    public string BuildingAlternateName { get; set; }
    public string BuildingAddress { get; set; }
    public ICollection<string> Contacts { get; set; }
    public string CountryName { get; set; }
    public string TerritoryName { get; set; }
    public string SalesName { get; set; }
    public string LastVisitDetails { get; set; }
}