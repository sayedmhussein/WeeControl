using System;
using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.RequestsResponses
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

        [Obsolete]
        public ResponseDto(HttpStatusCode httpStatuesCode) : base(httpStatuesCode)
        {
        }

        public T Payload { get; set; }
    }
}
