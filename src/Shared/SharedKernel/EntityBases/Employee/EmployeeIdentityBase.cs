using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.Enumerators.Employee;

namespace MySystem.SharedKernel.EntityBases.Employee
{
    public abstract class EmployeeIdentityBase
    {
        public IdentityTypeEnum IdentityType { get; set; }

        [Required]
        public string IdentityValue { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Only ISO 3166-1 alpha-3 Country Codes.")]
        public string CountryIssued { get; set; }

        public DateTime? DateIssed { get; set; }

        public DateTime? DateExpired { get; set; }
    }
}
