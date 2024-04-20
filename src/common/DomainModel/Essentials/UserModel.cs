using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.CustomValidationAttributes;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DomainModel.Essentials;

public class UserModel : IValidatableModel
{
    [Required]
    [StringLength(500, MinimumLength = 3)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [StandardString]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}