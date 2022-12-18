using WeeControl.Common.SharedKernel.Contexts.Business.Elevator.Enums;
using WeeControl.Common.SharedKernel.Contexts.Business.Elevator.Interfaces;

namespace WeeControl.Frontend.AppService.Internals.Temporary.Business.Elevator;

public class PortfolioModel : IPortfolioModel
{
    public string ContractNo { get; set; } = string.Empty;
    public ContractTypeEnum ContractType { get; set; }
    public ContractStateEnum ContractState { get; set; }
    public string UnitNo { get; set; } = string.Empty;
    public UnitTypeEnum UnitType { get; set; }
    public UnitStateTypeEnum UnitState { get; set; }
    public string BuildingName { get; set; } = string.Empty;
    public string BuildingAlternateName { get; set; } = string.Empty;
    public string BuildingAddress { get; set; } = string.Empty;
    public ICollection<string> Contacts { get; set; } = new List<string>();
    public string CountryName { get; set; } = string.Empty;
    public string TerritoryName { get; set; } = string.Empty;
    public string SalesName { get; set; } = string.Empty;
    public string LastVisitDetails { get; set; } = string.Empty;
}