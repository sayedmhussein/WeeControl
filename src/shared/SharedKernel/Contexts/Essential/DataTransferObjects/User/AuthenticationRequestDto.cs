using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects.User;

public class AuthenticationRequestDto
{
    [Required]
    [StringLength(45, MinimumLength = 3)]
    public string UsernameOrEmail { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public static AuthenticationRequestDto Create(string usernameOrEmail, string password)
    {
        return new AuthenticationRequestDto()
        {
            UsernameOrEmail = usernameOrEmail, Password = password
        };
    }
}