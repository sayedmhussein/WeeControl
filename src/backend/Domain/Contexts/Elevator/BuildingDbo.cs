using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Contexts.Elevator;

public class BuildingDbo
{
    public Guid BuildingId { get; set; }
    [MinLength(3)] public string CountryId { get; set; }
    public string BuildingName { get; set; }
    [MinLength(-90), MaxLength(90)] public double? Latitude { get; set; }
    [MinLength(-180), MaxLength(180)] public double? Longitude { get; set; }

    [StringLength(3, MinimumLength = 3)] public string BuildingType { get; set; }

    public string OfficeId { get; set; }
    public virtual IEnumerable<UnitDbo> Units { get; set; }
}