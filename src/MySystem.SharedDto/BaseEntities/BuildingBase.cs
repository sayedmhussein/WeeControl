using System;
using System.ComponentModel.DataAnnotations;

namespace Sayed.MySystem.SharedDto.BaseEntities
{
    public abstract class BuildingBase
    {
        public int? BuildingType { get; set; }

        [Required]
        public string BuildingName { get; set; }

        [Required]
        public string CountryId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
