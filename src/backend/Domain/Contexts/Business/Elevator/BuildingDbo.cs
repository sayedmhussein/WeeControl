using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Contexts.Business.Elevator.Enums;
using WeeControl.SharedKernel.Contexts.Business.Elevator.Interfaces;

namespace WeeControl.ApiApp.Domain.Contexts.Business.Elevator;

public class BuildingDbo : IBuildingModel
{
    public Guid BuildingId { get; set; }
    
    [MinLength(3)] 
    public string CountryId { get; set; }
    
    public string BuildingAddress { get; set; }
    public string BuildingName { get; set; }
    
    [MinLength(-90), MaxLength(90)] 
    public double? Latitude { get; set; }
    [MinLength(-180), MaxLength(180)] 
    public double? Longitude { get; set; }
    public BuildingTypeEnum BuildingType { get; set; }

    public string OfficeId { get; set; }
    public virtual IEnumerable<UnitDbo> Units { get; set; }
}