using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DataTransferObject.BodyObjects;

public class RequestDto : IRequestDto, IEntityModel
{
    internal RequestDto()
    {
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

    [Required] public string DeviceId { get; set; } = string.Empty;

    [Range(-90.0, 90.0)] public double? Latitude { get; set; }

    [Range(-180.0, 180.0)] public double? Longitude { get; set; }

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
}

public class RequestDto<T> : RequestDto, IRequestDto<T> where T : class
{
    internal RequestDto()
    {
        Payload = null!;
    }

    internal RequestDto(T payload, RequestDto dto) : base(dto)
    {
        Payload = payload;
    }

    public T Payload { get; init; }
}