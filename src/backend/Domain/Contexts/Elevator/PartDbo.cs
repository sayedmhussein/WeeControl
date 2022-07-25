using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Contexts.Elevator;

public class PartDbo
{
    [Key]
    public string PartNumber { get; set; }

    public string PartName { get; set; }

    public string PartReplacedBy { get; set; }

    public decimal BaseCost { get; set; }
}