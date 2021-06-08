using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Employee
{
    public class LoginDto : IDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum Username is 3 letters")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Minimum Password is 3 letters")]
        public string Password { get; set; }

        public string Device { get; set; }
    }
}
