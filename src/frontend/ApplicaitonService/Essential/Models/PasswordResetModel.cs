using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Frontend.ApplicationService.Essential.Models;

public class PasswordResetModel
{
    [Required]
    [MaxLength(45)]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username length is between 3 and 50 letters.")]
    [DisplayName("Username")]
    public string Username { get; set; } = string.Empty;
}