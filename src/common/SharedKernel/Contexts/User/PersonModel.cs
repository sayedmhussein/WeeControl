using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class PersonModel
{
    [Required]
    [StringLength(45, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;
    
    [StringLength(45)]
    public string? SecondName { get; set; } = string.Empty;
    
    [StringLength(45)]
    public string? ThirdName { get; set; } = string.Empty;

    [Required]
    [StringLength(45, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(3, MinimumLength = 3)]
    public string Nationality { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
}