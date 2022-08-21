using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.SharedKernel.Contexts.Essential.Entities;

public class UserEntity
{
    [Required]
    [StringLength(500, MinimumLength = 3)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [Phone] 
    [StringLength(20)]
    public string MobileNo { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)] 
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(128, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    
    [NotMapped]
    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; } = string.Empty;
}