using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class CustomerModel
{
    [Required]
    [StringLength(25)]
    public string CustomerName { get; set; } = string.Empty;

    [StringLength(25)]
    public string? CustomerLocalName { get; set; } = string.Empty;
    
    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; } = string.Empty;

    [StringLength(255)]
    public string CustomerAddress { get; set; } = string.Empty;
    
    [StringLength(255)]
    public string InvoiceAddress { get; set; } = string.Empty;
}