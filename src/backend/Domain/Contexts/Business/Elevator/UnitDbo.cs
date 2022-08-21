using System;
using WeeControl.SharedKernel.Contexts.Business.Elevator.Enums;
using WeeControl.SharedKernel.Contexts.Business.Elevator.Interfaces;

namespace WeeControl.ApiApp.Domain.Contexts.Business.Elevator;

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