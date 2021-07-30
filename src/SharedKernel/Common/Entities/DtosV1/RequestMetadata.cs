using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Common.DtosV1
{
    public class RequestMetadataV1 : IRequestMetadata, IDto
    {
        [Required]
        public string Device { get; set ; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
