using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.Api.Dtos
{
    public class RequestV1Dto<T>
    {
        public T Payload { get; set; }

        [Required]
        public string DeviceId { get; set; }

        public DateTime DeviceTs { get; set; }

        public Guid UUID { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
