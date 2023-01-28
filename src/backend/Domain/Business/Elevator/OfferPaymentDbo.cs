using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.Domain.Business.Elevator;

public class OfferPaymentDbo
{
    [Key]
    public Guid OfferPaymentId { get; set; }

    public Guid OfferId { get; set; }
    public OfferDbo Offer { get; set; }

}