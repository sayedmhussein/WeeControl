using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.Contexts.Temporary.Entities;

public class UserIdentityEntity
{
    [Required] [StringLength(25)] 
    public string Type { get; set; } = string.Empty;
    
    [Required]
    [StringLength(25)]
    public string Number { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpireDate { get; set; } = null;

    [Required]
    [StringLength(3, MinimumLength = 3)] 
    public string CountryId { get; set; } = string.Empty;
}