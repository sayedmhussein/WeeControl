using WeeControl.SharedKernel.Contexts.Elevator.Enums;

namespace WeeControl.SharedKernel.Contexts.Elevator.Interfaces;

public interface IUnitModel
{ 
    string UnitNumber { get; set; }
    UnitBrandEnum UnitBrand { get; set; }
    UnitTypeEnum  UnitType { get; set; }
    UnitStateTypeEnum UnitState { get; set; }

    string UnitIdentification { get; set; }
}