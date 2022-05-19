using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class LoginDtoV1
{
    [Required]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required] 
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;

    public LoginDtoV1()
    {
    }
        
    public LoginDtoV1(string usernameOrEmail, string password)
    {
        UsernameOrEmail = usernameOrEmail;
        Password = password;
    }
}