using System.ComponentModel.DataAnnotations;
using WeeControl.Common.BoundedContext.Credentials.BaseObjects;

namespace WeeControl.Common.BoundedContext.Credentials.DataTransferObjects
{
    public class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
