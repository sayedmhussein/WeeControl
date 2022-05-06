using System.ComponentModel.DataAnnotations;
using WeeControl.Common.BoundedContext.Credentials.BaseObjects;

namespace WeeControl.Common.BoundedContext.Credentials.DataTransferObjects
{
    public class LoginDto
    {
        [Required]
        [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required] public string Password { get; set; } = string.Empty;
    }
}
