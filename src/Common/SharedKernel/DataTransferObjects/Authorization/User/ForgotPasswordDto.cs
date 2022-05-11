using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User
{
    public class ForgotPasswordDto
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
    }
}
