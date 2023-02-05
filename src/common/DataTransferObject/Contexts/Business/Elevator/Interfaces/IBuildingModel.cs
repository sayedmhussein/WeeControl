using WeeControl.Core.DataTransferObject.Contexts.Business.Elevator.Enums;

namespace WeeControl.Core.DataTransferObject.Contexts.Business.Elevator.Interfaces;

public interface IBuildingModel
{
    Guid BuildingId { get; set; }
    string BuildingName { get; set; }
    BuildingTypeEnum BuildingType { get; set; }
    string CountryId { get; set; }
    string BuildingAddress { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

}