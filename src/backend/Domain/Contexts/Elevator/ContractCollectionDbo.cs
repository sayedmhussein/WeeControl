using System;

namespace WeeControl.Domain.Contexts.Elevator;

public class ContractCollectionDbo
{
    public Guid ContractCollectionId { get; set; }

    public string ContractId { get; set; }
    public ContractDbo Contract { get; set; }

    public decimal Amount { get; set; }
}