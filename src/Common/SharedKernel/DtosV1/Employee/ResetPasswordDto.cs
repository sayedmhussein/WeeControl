using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.DtosV1.Employee
{
    public class ResetPasswordDto : IEntityDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username length is between 3 and 50 letters.")]
        [DisplayName("Username")]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }
    }
}