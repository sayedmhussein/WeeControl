using System;
using System.ComponentModel.DataAnnotations;

namespace Sayed.MySystem.Shared.Entities
{
    public abstract class PersonBase
    {
        [StringLength(10, ErrorMessage = "Always use english common titles not exceeding 10 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(45)]
        public string SecondName { get; set; }

        [StringLength(45)]
        public string ThirdName { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(1, ErrorMessage = "Use either m or f or keep it null.")]
        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(3)]
        public string Nationality { get; set; }

        [StringLength(3)]
        public string Language { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Mobile { get; set; }

        [MaxLength(50)]
        public string PhotoUrl { get; set; }
    }
}
