namespace WeeControl.SharedKernel.Interfaces;

public interface IRequestDto
{
    string DeviceId { get; }

    double? Latitude { get; }

    double? Longitude { get; }
}

public interface IRequestDto<T> : IRequestDto where T : class
{
    T Payload { get; }
}