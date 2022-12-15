using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.RequestsResponses;

public class RequestDto : IRequestDto
{
    public static IRequestDto Create(string device, double? latitude, double? longitude)
    {
        return new RequestDto(device, latitude, longitude);
    }

    public static IRequestDto<T> Create<T>(T payload, string device, double? latitude, double? longitude) where T : class
    {
        return new RequestDto<T>(payload, new RequestDto(device, latitude, longitude));
    }
    
    public static IRequestDto<T> Create<T>(T payload, IRequestDto dto) where T : class
    {
        return new RequestDto<T>(payload,dto);
    }

    public string DeviceId { get; }

    public double? Latitude { get; }

    public double? Longitude { get; }

    internal RequestDto()
    {
        DeviceId = string.Empty;
    }
    
    protected RequestDto(IRequestDto dto) : this(dto.DeviceId, dto.Latitude, dto.Longitude)
    {
    }
    
    private RequestDto(string deviceId, double? latitude, double? longitude)
    {
        DeviceId = deviceId;
        Latitude = latitude;
        Longitude = longitude;
    }
}

public class RequestDto<T> : RequestDto, IRequestDto<T> where T : class
{
    public T Payload { get; }

    internal RequestDto()
    {
        Payload = null!;
    }
    
    internal RequestDto(T payload, IRequestDto dto) : base(dto)
    {
        Payload = payload;
    }
}