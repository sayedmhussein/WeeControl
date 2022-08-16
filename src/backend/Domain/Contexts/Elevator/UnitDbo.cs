using System;
using WeeControl.SharedKernel.Contexts.Elevator.Enums;
using WeeControl.SharedKernel.Contexts.Elevator.Interfaces;

namespace WeeControl.Domain.Contexts.Elevator;

public class UnitDbo : IUnitModel
{
    public string UnitNumber { get; set; }
    public UnitBrandEnum UnitBrand { get; set; }

    public UnitStateTypeEnum UnitState { get; set; }

    public Guid BuildingId { get; set; }
    public BuildingDbo Building { get; set; }

    public UnitTypeEnum UnitType { get; set; }
    public string UnitIdentification { get; set; }
}