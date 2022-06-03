namespace WeeControl.SharedKernel.Interfaces;

public interface IRequestDto
{
    string DeviceId { get; }

    double? Latitude { get; }

    double? Longitude { get; }
}