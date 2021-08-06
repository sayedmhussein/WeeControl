using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.DtosV1
{
    public class CreateLoginDto : ISerializable
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum Username is 3 letters")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Minimum Password is 3 letters")]
        public string Password { get; set; }
    }
}
