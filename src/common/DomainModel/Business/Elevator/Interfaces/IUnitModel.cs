using WeeControl.Core.DomainModel.Business.Elevator.Enums;

namespace WeeControl.Core.DomainModel.Business.Elevator.Interfaces;

public interface IUnitModel
{
    string UnitNumber { get; set; }
    UnitBrandEnum UnitBrand { get; set; }
    UnitTypeEnum UnitType { get; set; }
    UnitStateTypeEnum UnitState { get; set; }

    string UnitIdentification { get; set; }
}