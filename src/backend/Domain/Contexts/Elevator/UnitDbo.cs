using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Elevator.Constants;
using WeeControl.SharedKernel.Elevator.Enums;
using WeeControl.SharedKernel.Elevator.Interfaces;

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