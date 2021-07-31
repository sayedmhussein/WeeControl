using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Common.DtosV1
{
    [Obsolete]
    public class RequestMetadataV1 : IRequestMetadata, IAggregateRoot
    {
        [Required]
        public string Device { get; set ; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
