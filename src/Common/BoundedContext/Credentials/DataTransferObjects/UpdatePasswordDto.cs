using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Common.BoundedContext.Credentials.DataTransferObjects
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Password length is between 3 and 50 letters.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
