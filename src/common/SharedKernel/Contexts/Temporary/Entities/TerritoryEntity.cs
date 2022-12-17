using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.Contexts.Temporary.Entities;

public class TerritoryEntity
{
    [Required]
    [StringLength(255, MinimumLength = 3)]
    public string UniqueName { get; set; } = string.Empty;
    
    [StringLength(255)]
    public string? AlternativeName { get; set; } = string.Empty;
    
    [StringLength(3, MinimumLength = 3)] 
    public string CountryCode { get; set; } = string.Empty;
}