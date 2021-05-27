using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.Shared.Library.Base
{
    public abstract class BuildingBase
    {
        public int? BuildingType { get; set; }

        [Required]
        [MinLength(3)]
        public string BuildingName { get; set; }

        [Required]
        [StringLength(3)]
        public string CountryId { get; set; }

        [StringLength(50)]
        public string AddressLine1 { get; set; }

        [StringLength(50)]
        public string AddressLine2 { get; set; }

        [Range(-90.0, 90.0)]
        public double? Latitude { get; set; }

        [Range(-180.0, 180.0)]
        public double? Longitude { get; set; }
    }
}
