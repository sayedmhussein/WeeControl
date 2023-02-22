using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.Essentials;

public class CustomerModel : IEntityModel
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