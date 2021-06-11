using System;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Common
{
    public class RequestMetadata : IRequestMetadata
    {
        public string Device { get; set ; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
