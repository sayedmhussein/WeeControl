using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Common.DtosV1
{
    public class RequestMetadata : IRequestMetadata, IDto
    {
        [Required]
        public string Device { get; set ; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
