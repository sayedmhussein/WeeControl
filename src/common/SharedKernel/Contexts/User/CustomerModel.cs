using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class CustomerModel
{
    [Required]
    [StringLength(25)]
    public string CompanyName { get; set; } = string.Empty;

    [StringLength(25)]
    public string? CompanyLocalName { get; set; } = string.Empty;
    
    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; } = string.Empty;
}