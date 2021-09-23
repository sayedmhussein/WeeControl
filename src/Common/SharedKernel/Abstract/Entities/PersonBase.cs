using System;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.Abstract.Entities
{
    public abstract class PersonBase
    {
        [StringLength(5, ErrorMessage = "Maximum length is 5 letters.")]
        public string Title { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name cannot be less than 3 chars or longer than 45 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string SecondName { get; set; }

        [StringLength(50)]
        public string ThirdName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name cannot be less than 3 chars or longer than 45 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public DateTime? DateOfBirth { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Nationality { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Language { get; set; }

        [StringLength(1, MinimumLength = 1, ErrorMessage = "Either use 'm' for male or 'f' for femail.")]
        public string Gender { get; set; }
    }
}