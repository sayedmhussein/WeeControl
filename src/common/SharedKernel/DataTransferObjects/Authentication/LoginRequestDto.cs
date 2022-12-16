using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.DataTransferObjects.Authentication;

public class LoginRequestDto
{
    [Required]
    [StringLength(45, MinimumLength = 3)]
    public string UsernameOrEmail { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public static LoginRequestDto Create(string usernameOrEmail, string password)
    {
        return new LoginRequestDto()
        {
            UsernameOrEmail = usernameOrEmail.Trim(), Password = password
        };
    }
}