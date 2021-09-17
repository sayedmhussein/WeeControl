using System.Net;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.DtosV1
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
