using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.CustomValidationAttributes;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DomainModel.Essentials;

public class PersonModel : IValidatableModel
{
    [Required]
    [StringLength(45, MinimumLength = 1)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(45)]
    [Display(Name = "Second Name")]
    public string? SecondName { get; set; } = string.Empty;

    [StringLength(45)] public string? ThirdName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Last Name")]
    [StringLength(45, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(3, MinimumLength = 3)]
    public string? NationalityCode { get; set; }

    [Required] public DateTime? DateOfBirth { get; set; }

    [StringLength(500, MinimumLength = 3)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(255)] [StandardString] public string? Username { get; set; }

    [StringLength(128, MinimumLength = 6)] public string? Password { get; set; }
}