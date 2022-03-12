using System.Net;
using WeeControl.Common.BoundedContext.Interfaces;

namespace WeeControl.Common.BoundedContext.RequestsResponses
{
    public class ResponseDto<T> : ResponseDto, IResponseDto<T> where T : class
    {
        public ResponseDto()
        {
        }

        public ResponseDto(T payload)
        {
            Payload = payload;
        }

        public ResponseDto(HttpStatusCode httpStatuesCode) : base(httpStatuesCode)
        {
        }

        public T Payload { get; set; }
    }
}
