namespace WeeControl.Common.SharedKernel.Interfaces;

public interface IRequestDto
{
    string DeviceId { get; set; }

    double? Latitude { get; set; }

    double? Longitude { get; set; }
}