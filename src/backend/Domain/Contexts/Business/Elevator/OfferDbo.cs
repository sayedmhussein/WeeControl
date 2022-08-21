using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.ApiApp.Domain.Contexts.Business.Elevator;

public class OfferDbo
{
    [Key]
    public Guid OfferId { get; set; }

    public Guid CustomerId { get; set; }
    public CustomerDbo Customer { get; set; }

    public virtual IEnumerable<OfferLaborDbo> EstimatedLabor { get; set; }
    public virtual IEnumerable<OfferMaterialsDbo> EstimatedMaterial { get; set; }
    public virtual IEnumerable<OfferPaymentDbo> Payments { get; set; }
}