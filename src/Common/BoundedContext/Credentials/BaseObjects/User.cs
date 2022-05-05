using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.BoundedContext.Credentials.BaseObjects
{
    public abstract class User
    {
        // [EmailAddress]
        // public string Email { get; set; }

        [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
