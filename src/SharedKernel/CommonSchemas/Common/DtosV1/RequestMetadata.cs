using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.CommonSchemas.Common.DtosV1
{
    public class RequestMetadata : IRequestMetadata, IDto
    {
        [Required]
        public string Device { get; set ; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
