using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User
{
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
    }
}
