using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.RequestsResponses
{
    public class ResponseDto<T> : ResponseDto, IResponseDto<T> where T : class
    {
        private ResponseDto() : base()
        {
        }

        public ResponseDto(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; set; }
    }
}
