using System;
namespace MySystem.Shared.Library.Dtos
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
