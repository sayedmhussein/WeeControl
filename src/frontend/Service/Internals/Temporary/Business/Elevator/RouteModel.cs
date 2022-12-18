using WeeControl.Common.SharedKernel.Contexts.Business.Elevator.Enums;
using WeeControl.Common.SharedKernel.Contexts.Business.Elevator.Interfaces;

namespace WeeControl.Frontend.AppService.Internals.Temporary.Business.Elevator;

public class RouteModel : IRouteModel
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
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public VisitTypeEnum VisitType { get; set; }
    public int VisitPriorityScale { get; set; }
    public ICollection<string> Contacts { get; set; } = new List<string>();
    public string SpecialInstructions { get; set; } = string.Empty;
}