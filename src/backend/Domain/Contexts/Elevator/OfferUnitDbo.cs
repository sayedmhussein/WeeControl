using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Contexts.Elevator;

public class OfferUnitDbo
{
    [Key]
    public Guid OfferId { get; set; }

    [Key]
    public string UnitNumber { get; set; }
}