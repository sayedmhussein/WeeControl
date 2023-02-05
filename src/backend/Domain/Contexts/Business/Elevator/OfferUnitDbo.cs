using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.Domain.Contexts.Business.Elevator;

public class OfferUnitDbo
{
    [Key]
    public Guid OfferId { get; set; }

    [Key]
    public string UnitNumber { get; set; }
}