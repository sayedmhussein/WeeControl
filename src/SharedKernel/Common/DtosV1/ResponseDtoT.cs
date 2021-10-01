using System.Net;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Common.DtosV1
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
