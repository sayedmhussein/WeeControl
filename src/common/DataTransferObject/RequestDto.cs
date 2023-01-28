using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.DataTransferObject;

public class RequestDto
{
    public static RequestDto Create(string device, double? latitude, double? longitude)
    {
        return new RequestDto(device, latitude, longitude);
    }

    public static RequestDto<T> Create<T>(T payload, string device, double? latitude, double? longitude) where T : class
    {
        return new RequestDto<T>(payload, new RequestDto(device, latitude, longitude));
    }

    public static RequestDto<T> Create<T>(T payload, RequestDto dto) where T : class
    {
        return new RequestDto<T>(payload, dto);
    }

    [Required]
    public string DeviceId { get; init; }

    public double? Latitude { get; init; }

    public double? Longitude { get; init; }

    internal RequestDto()
    {
        DeviceId = string.Empty;
    }

    protected RequestDto(RequestDto dto) : this(dto.DeviceId, dto.Latitude, dto.Longitude)
    {
    }

    private RequestDto(string deviceId, double? latitude, double? longitude)
    {
        DeviceId = deviceId;
        Latitude = latitude;
        Longitude = longitude;
    }
}

public class RequestDto<T> : RequestDto where T : class
{
    public T Payload { get; init; }

    internal RequestDto()
    {
        Payload = null!;
    }

    internal RequestDto(T payload, RequestDto dto) : base(dto)
    {
        Payload = payload;
    }
}