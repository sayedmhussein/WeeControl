using WeeControl.Common.BoundedContext.Interfaces;

namespace WeeControl.Common.BoundedContext.RequestsResponses
{
    public class RequestDto<T> : RequestDto, IRequestDto<T> where T : class
    {
        public T Payload { get; set; }

        public RequestDto()
        {
        }
        
        public RequestDto(string device, T payload) : base(device)
        {
            Payload = payload;
        }

        public RequestDto(string device, T payload, double latitude, double longitude) : base(device, latitude, longitude)
        {
            Payload = payload;
        }
    }
}
