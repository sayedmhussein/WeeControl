using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.Domain.Contexts.Business.Elevator;

public class PartDbo
{
    [Key] public Guid PartId { get; set; } = Guid.NewGuid();

    public Guid? ReplacedById { get; set; }

    public Guid VendorId { get; set; }
    public VendorDbo Vendor { get; set; }

    [StringLength(50)] [Required] public string PartNo { get; set; }

    [StringLength(50)] [Required] public string PartName { get; set; }

    public bool IsObsolute { get; set; } = false;

    [Required] public int PartType { get; set; }

    [StringLength(255)] public string Description { get; set; }

    [StringLength(255)] public string Keywords { get; set; }

    [Required] public decimal BasePrice { get; set; }
}