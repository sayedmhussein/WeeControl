using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Authorization.Bases
{
    public abstract class UserBase
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username length is between 3 and 50 letters.")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        public string UserEmail { get; set; }

        [Phone]
        [DisplayName("Mobile")]
        public string UserMobile { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Password length is between 3 and 50 letters.")]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}