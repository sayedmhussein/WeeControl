using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySystem.Shared.Library.Dto.EntityV1
{
    public class LoginDto : IEntityDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum Username is 3 letters")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Minimum Password is 3 letters")]
        public string Password { get; set; }

        public Guid? Id { get; set; }
    }
}
