using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Contexts.Elevator;

public class UnitDbo
{
    public string UnitNumber { get; set; }
    public Guid BuildingId { get; set; }
    public BuildingDbo Building { get; set; }
    public string UnitIdentification { get; set; }
}