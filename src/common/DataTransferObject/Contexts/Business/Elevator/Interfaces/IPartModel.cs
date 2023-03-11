using WeeControl.Core.DataTransferObject.Contexts.Business.Elevator.Enums;

namespace WeeControl.Core.DataTransferObject.Contexts.Business.Elevator.Interfaces;

public interface IPartModel
{
    public Guid PartId { get; set; }
    public Guid? ReplacedById { get; set; }
    public Guid VendorId { get; set; }
    public string PartNo { get; set; }
    public string PartName { get; set; }
    public bool IsObsolute { get; set; }
    public PartTypeEnum PartType { get; set; }
    public string Description { get; set; }
    public string Keywords { get; set; }
    public decimal BasePrice { get; set; }
}