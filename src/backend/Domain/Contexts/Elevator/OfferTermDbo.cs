using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Contexts.Elevator;

public class OfferTermDbo
{
    [Key]
    public Guid OfferId { get; set; }

    [Key]
    public int TermId { get; set; }
}