using WeeControl.Core.DomainModel.Business.Elevator.Enums;

namespace WeeControl.Core.DomainModel.Business.FieldOperation;

public class RouteSheetDto
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