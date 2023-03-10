using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.Domain.Contexts.Business.Elevator;

public class OfferTermDbo
{
    [Key] public Guid OfferId { get; set; }

    [Key] public int TermId { get; set; }
}