using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.ApiApp.Domain.Contexts.Business.Elevator;

public class CustomerDbo
{
    [Key]
    public Guid CustomerId { get; set; }
}