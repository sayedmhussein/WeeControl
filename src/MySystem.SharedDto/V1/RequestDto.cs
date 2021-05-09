using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedDto.Interfaces;

namespace MySystem.SharedDto.V1
{
    public class RequestDto<T> : IRequestDto<T>
    {
        public T Payload { get; set; }

        [Required]
        public string DeviceId { get; set; }

        public DateTime DeviceTs { get; set; } = DateTime.UtcNow;

        public Guid UUID { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public RequestDto()
        {
        }

        public RequestDto(string deviceId)
        {
            DeviceId = deviceId;
        }

        public RequestDto(T payload)
        {
            Payload = payload;
        }

        public RequestDto(string deviceId, T payload) : this(payload)
        {
            DeviceId = deviceId;
        }
    }
}
