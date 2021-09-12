using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.EntityGroup.Employee.Enums;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroup.Employee.BaseEntities
{
    public abstract class BaseEmployeeIdentity : IVerifyable
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
