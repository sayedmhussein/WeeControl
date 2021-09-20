using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1.Authorization
{
    public class RequestNewTokenDto : IEntityDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username length is between 3 and 50 letters.")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Password length is between 3 and 50 letters.")]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
