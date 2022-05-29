using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.DataTransferObjects;

public class UserDto
{
    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; }
    
    [Required]
    [MinLength(3)]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    [DisplayName("Username")]
    public string Username { get; set; }

    
    public string TerritoryCode { get; set; }
    
    
}