using System;

namespace WeeControl.Core.Domain.Business.Elevator;

public class ContractMaterialDbo
{
    public Guid ContractMaterialId { get; set; }

    public string ContractId { get; set; }
    public ContractDbo Contract { get; set; }

    public string PartNumber { get; set; }
    public PartDbo Part { get; set; }

    public decimal ActualCost { get; set; }
}