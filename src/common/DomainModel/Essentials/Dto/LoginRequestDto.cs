using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.CustomValidationAttributes;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DomainModel.Essentials.Dto;

public class LoginRequestDto : IEntityModel
{
    [Required(ErrorMessage = "You must enter either your username or your email.")]
    [StringLength(45, MinimumLength = 3, ErrorMessage = "Username or email should be between 3 and 45 character.")]
    [StandardString("@.-")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public static LoginRequestDto Create(string usernameOrEmail, string password)
    {
        return new LoginRequestDto
        {
            UsernameOrEmail = usernameOrEmail.Trim(),
            Password = password
        };
    }
}