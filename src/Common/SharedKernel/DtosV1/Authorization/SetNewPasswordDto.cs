using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1.Authorization
{
    public class SetNewPasswordDto : IEntityDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Password length is between 3 and 50 letters.")]
        [DisplayName("Password")]
        public string Password { get; set; }

        public string Password_ { get; set; }

        public string TemporaryToken { get; set; }
    }
}