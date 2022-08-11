using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.Entities;

public class UserIdentityEntity
{
    [Required]
    [StringLength(25)]
    public string Type { get; set; }
    
    [Required]
    [StringLength(25)]
    public string Number { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    [StringLength(3, MinimumLength = 3)] 
    public string CountryId { get; set; } = string.Empty;
}