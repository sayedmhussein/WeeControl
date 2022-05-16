using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.RequestDTOs;

public class LoginDto
{
    [Required]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required] 
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;

    public LoginDto()
    {
    }
        
    public LoginDto(string usernameOrEmail, string password)
    {
        UsernameOrEmail = usernameOrEmail;
        Password = password;
    }
    
    public static class HttpPostMethod
    {
        public const string EndPoint = "Api/Credentials";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
    }
}