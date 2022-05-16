using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.RequestDTOs;

public class PasswordResetRequestDto
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username length is between 3 and 50 letters.")]
    [DisplayName("Username")]
    public string Username { get; set; }

    [Required]
    [MaxLength(45)]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; }
    
    public static class HttpPostMethod
    {
        public const string EndPoint = "Api/Credentials/reset";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
    }
}