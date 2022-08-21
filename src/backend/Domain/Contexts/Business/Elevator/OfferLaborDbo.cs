using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.ApiApp.Domain.Contexts.Business.Elevator;

public class OfferLaborDbo
{
    [Key]
    public Guid OfferLaborId { get; set; }

    public Guid OfferId { get; set; }
    public OfferDbo Offer { get; set; }
}