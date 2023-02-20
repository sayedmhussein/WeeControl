using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class PersonModel : IEntityModel
{
    [Required]
    [StringLength(45, MinimumLength = 1)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    [Display(Name = "Second Name")]
    [StringLength(45)]
    public string? SecondName { get; set; } = string.Empty;
    
    [StringLength(45)]
    public string? ThirdName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Last Name")]
    [StringLength(45, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(3, MinimumLength = 3)]
    public string NationalityCode { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
}