using System;

namespace WeeControl.Core.Domain.Business.Elevator;

public class UnitDbo
{
    public string UnitNumber { get; set; }
    public int UnitBrand { get; set; }

    public int UnitState { get; set; }

    public Guid BuildingId { get; set; }
    public BuildingDbo Building { get; set; }

    public int UnitType { get; set; }
    public string UnitIdentification { get; set; }
}