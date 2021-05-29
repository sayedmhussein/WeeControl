using System;
namespace MySystem.SharedKernel.Interface
{
    public interface IRequestDto<T>
    {
        T Payload { get; set; }
        string DeviceId { get; set; }

        DateTime DeviceTs { get; set; }
        Guid UUID { get; set; }

        double? Latitude { get; set; }
        double? Longitude { get; set; }
    }
}
