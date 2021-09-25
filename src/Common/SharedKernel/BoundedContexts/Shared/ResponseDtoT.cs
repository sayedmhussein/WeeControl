using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.BoundedContexts.Shared
{
    public class ResponseDto<T> : IResponseDto<T> where T : class
    {
        public ResponseDto(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; set; }
        
        public HttpStatusCode HttpStatuesCode { get; set; }
    }
}
