using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroups.Territory
{
    public abstract class BaseTerritory : IVerifyable
    {
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Only ISO 3166-1 alpha-3 Country Codes of 3 letters.")]
        public string CountryId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Minimum Name must be 3 letters.")]
        [StringLength(45, ErrorMessage = "Office name must not exceed 45 character.")]
        public string Name { get; set; }

        public Guid? ReportToId { get; set; }
    }
}
