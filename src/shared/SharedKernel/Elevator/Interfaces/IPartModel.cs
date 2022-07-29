using WeeControl.SharedKernel.Elevator.Enums;

namespace WeeControl.SharedKernel.Elevator.Interfaces;

public interface IPartModel
{
    public Guid PartId { get; set; }
    public string PartNo { get; set; }
    public Guid? ReplacedById { get; set; }
    public bool IsObsolute { get; set; }
    public PartTypeEnum PartType { get; set; }
    public PartSourceEnum PartSource { get; set; }
    
}