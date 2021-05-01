using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.SharedDto.BaseEntities
{
    public abstract class OfficeBase
    {
        [StringLength(3, ErrorMessage = "Only ISO 3166-1 alpha-3 Country Codes.")]
        [MinLength(3), MaxLength(3)]
        public string CountryId { get; set; }

        [StringLength(45, ErrorMessage = "Office name must not exceed 45 character.")]
        public string OfficeName { get; set; }
    }
}
