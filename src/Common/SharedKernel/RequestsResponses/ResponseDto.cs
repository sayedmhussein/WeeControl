using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.RequestsResponses
{
    public class ResponseDto : IResponseDto
    {
        [Obsolete]
        public HttpStatusCode StatuesCode { get; set; }

        public ResponseDto()
        {
        }

        [Obsolete]
        public ResponseDto(HttpStatusCode httpStatuesCode)
        {
            StatuesCode = httpStatuesCode;
        }
    }
}