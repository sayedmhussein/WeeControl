using WeeControl.Common.SharedKernel.Contexts.Business.Elevator.Enums;

namespace WeeControl.Common.SharedKernel.Contexts.Business.Elevator.Interfaces;

public interface IUnitModel
{ 
    string UnitNumber { get; set; }
    UnitBrandEnum UnitBrand { get; set; }
    UnitTypeEnum  UnitType { get; set; }
    UnitStateTypeEnum UnitState { get; set; }

    string UnitIdentification { get; set; }
}