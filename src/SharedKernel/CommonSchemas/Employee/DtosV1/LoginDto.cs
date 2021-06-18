using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.DtosV1
{
    public class CreateLoginDto : IRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum Username is 3 letters")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Minimum Password is 3 letters")]
        public string Password { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
