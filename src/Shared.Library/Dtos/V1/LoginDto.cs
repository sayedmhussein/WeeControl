using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySystem.Shared.Library.Dtos.V1
{
    public class LoginDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum Username is 3 letters")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Minimum Password is 3 letters")]
        public string Password { get; set; }
    }
}
