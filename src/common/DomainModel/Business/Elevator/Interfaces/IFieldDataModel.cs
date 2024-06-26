using WeeControl.Core.DomainModel.Business.Elevator.Enums;

namespace WeeControl.Core.DomainModel.Business.Elevator.Interfaces;

public interface IFieldDataModel
{
    string ContractNo { get; set; }
    ContractTypeEnum ContractType { get; set; }
    ContractStateEnum ContractState { get; set; }

    string UnitNo { get; set; }
    UnitTypeEnum UnitType { get; set; }
    UnitStateTypeEnum UnitState { get; set; }

    string BuildingName { get; set; }
    string BuildingAlternateName { get; set; }
    string BuildingAddress { get; set; }

    ICollection<string> Contacts { get; set; }
}