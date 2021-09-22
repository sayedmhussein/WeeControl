using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.Common.SharedKernel.Abstract.Enums;

namespace WeeControl.Common.SharedKernel.Abstract.Entities
{
    public interface IPerson
    {
        PersonTitle Title { get; set; }
        string FirstName { get; set; }
        string SecondName { get; set; }
        string ThirdName { get; set; }
        string LastName { get; set; }
        DateTime? DateOfBirth { get; set; }
        string Nationality { get; set; }
        string Language { get; set; }
        PersonGender Gender { get; set; }
    }

    public class PersonBase : IPerson
    {
        public PersonTitle Title { get; set; }
        
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

        public PersonGender Gender { get; set; }

        
    }
}