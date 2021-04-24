using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.Data.V1.Dtos
{
    public class RequestV1<T>
    {
        public T Payload { get; set; }

        [Required]
        public string DeviceId { get; set; }

        public DateTime DeviceTs { get; set; }

        public Guid UniqueIdentifier { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
