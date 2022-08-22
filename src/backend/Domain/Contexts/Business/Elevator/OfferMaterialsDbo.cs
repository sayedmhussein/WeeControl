using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.ApiApp.Domain.Contexts.Business.Elevator;

public class OfferMaterialsDbo
{
    [Key]
    public Guid OfferMaterialId { get; set; }

    public Guid OfferId { get; set; }
    public OfferDbo Offer { get; set; }

    public string PartNumber { get; set; }
    public decimal EstimatedCost { get; set; }
}