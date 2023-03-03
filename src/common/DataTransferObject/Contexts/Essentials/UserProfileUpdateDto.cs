using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class UserProfileUpdateDto
{
    [StringLength(45)]
    public string? SecondName { get; set; } = string.Empty;
    
    [StringLength(45)]
    public string? ThirdName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, MinimumLength = 3)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}