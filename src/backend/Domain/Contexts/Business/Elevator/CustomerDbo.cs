using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.Domain.Contexts.Business.Elevator;

public class CustomerDbo
{
    [Key]
    public Guid CustomerId { get; set; }
}