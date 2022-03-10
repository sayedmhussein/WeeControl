using System.Net;
using WeeControl.Common.BoundedContext.Interfaces;

namespace WeeControl.Common.BoundedContext.RequestsResponses
{
    public class ResponseDto<T> : IResponseDto<T> where T : class
    {
        public ResponseDto(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; set; }
        
        public HttpStatusCode StatuesCode { get; set; }
    }
}
