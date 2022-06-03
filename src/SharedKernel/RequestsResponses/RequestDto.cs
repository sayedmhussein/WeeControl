using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.RequestsResponses;

public class RequestDto : IRequestDto
{
    public static IRequestDto Create(string device, double? latitude, double? longitude)
    {
        return new RequestDto() { DeviceId = device, Latitude = latitude, Longitude = longitude};
    }

    public static IRequestDto<T> Create<T>(T payload, string device, double? latitude, double? longitude) where T : class
    {
        return new RequestDto<T>(device,payload,latitude,longitude);
    }
    
    public static IRequestDto<T> Create<T>(T payload, IRequestDto dto) where T : class
    {
        return new RequestDto<T>(dto.DeviceId,payload,dto.Latitude,dto.Longitude);
    }

    public string DeviceId { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    private RequestDto()
    {
        DeviceId = string.Empty;
    }

    [Obsolete("Use static method")]
    public RequestDto(string device) : this()
    {
        DeviceId = device;
    }

    [Obsolete("Use static method")]
    public RequestDto(string device, double? latitude, double? longitude) : this(device)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}

public class RequestDto<T> : RequestDto, IRequestDto<T> where T : class
{
    public T Payload { get; set; }

    private RequestDto() : base("")
    {
    }
    
    [Obsolete("Use static method")]
    public RequestDto(string device, T payload, double? latitude, double? longitude) : base(device, latitude, longitude)
    {
        Payload = payload;
    }
}