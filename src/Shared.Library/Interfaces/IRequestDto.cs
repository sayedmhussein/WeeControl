using System;
namespace MySystem.SharedKernel.Interfaces
{
    public interface IRequestDto<T> : IDto
    {
        T Payload { get; set; }
        string DeviceId { get; set; }

        DateTime DeviceTs { get; set; }
        Guid UUID { get; set; }

        double? Latitude { get; set; }
        double? Longitude { get; set; }
    }
}
