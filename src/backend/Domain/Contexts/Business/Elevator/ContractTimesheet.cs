using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.Domain.Contexts.Business.Elevator;

public class ContractTimesheet
{
    [Key] public Guid ContractTimesheetId { get; set; }

    public string ContractNumber { get; set; }
    public ContractDbo Contract { get; set; }
}