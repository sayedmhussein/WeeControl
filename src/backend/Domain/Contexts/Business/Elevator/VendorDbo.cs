using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Contexts.Business.Elevator;

public class VendorDbo
{
    [Key]
    public Guid VendorId { get; set; }
    
    
    public ICollection<PartDbo> Parts { get; set; }
}