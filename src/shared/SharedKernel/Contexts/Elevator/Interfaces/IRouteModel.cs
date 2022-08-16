using WeeControl.SharedKernel.Contexts.Elevator.Enums;

namespace WeeControl.SharedKernel.Contexts.Elevator.Interfaces;

public interface IRouteModel : IFieldDataModel
{
    double? Latitude { get; set; }
    double? Longitude { get; set; }
    
    VisitTypeEnum VisitType { get; set; }
    int VisitPriorityScale { get; set; }
    string SpecialInstructions { get; set; }
}