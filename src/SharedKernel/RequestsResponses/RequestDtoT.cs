using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.RequestsResponses;

public class RequestDto<T> : RequestDto, IRequestDto<T> where T : class
{
    public T Payload { get; set; }

    private RequestDto() : base()
    {
    }
        
    [Obsolete("Use other constructor which contains location parameters.")]
    public RequestDto(string device, T payload) : base(device)
    {
        Payload = payload;
    }

    public RequestDto(string device, T payload, double? latitude, double? longitude) : base(device, latitude, longitude)
    {
        Payload = payload;
    }
}