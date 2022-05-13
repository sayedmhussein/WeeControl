using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.DataTransferObjects.Essential.User
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        public RegisterDto()
        {
        }

        public static RegisterDto Create(string email, string username, string password)
        {
            return new RegisterDto() { Email = email, Username = username, Password = password };
        }
        
        public static class HttpPostMethod
        {
            public const string EndPoint = "Api/Credentials/Register";
            public const string Version = "1.0";
            public static string AbsoluteUri(string server) => server + EndPoint;
        }
    }
}
