using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Contexts.Elevator;

public class CustomerDbo
{
    [Key]
    public Guid CustomerId { get; set; }
}