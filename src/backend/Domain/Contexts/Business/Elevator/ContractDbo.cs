using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.Domain.Contexts.Business.Elevator;

public class ContractDbo
{
    [Key]
    public string ContractNumber { get; set; }

    public Guid OfferId { get; set; }
    public OfferDbo Offer { get; set; }

    [StringLength(3)][Required] public string ContractType { get; set; }

    public IEnumerable<ContractTimesheet> Timesheet { get; set; }
    public IEnumerable<ContractMaterialDbo> Materials { get; set; }
    public IEnumerable<ContractCollectionDbo> Collections { get; set; }
}