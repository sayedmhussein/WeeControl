using WeeControl.SharedKernel.Elevator.Enums;
using WeeControl.SharedKernel.Elevator.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Employee;

public class RouteModel : IRouteModel
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
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public VisitTypeEnum VisitType { get; set; }
    public int VisitPriorityScale { get; set; }
    public ICollection<string> Contacts { get; set; }
    public string SpecialInstructions { get; set; }
}