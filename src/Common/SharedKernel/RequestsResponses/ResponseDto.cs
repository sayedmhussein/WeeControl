using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.RequestsResponses
{
    public class ResponseDto : IResponseDto
    {
        public HttpStatusCode StatuesCode { get; set; }

        public ResponseDto()
        {
        }

        public ResponseDto(HttpStatusCode httpStatuesCode)
        {
            StatuesCode = httpStatuesCode;
        }
    }
}