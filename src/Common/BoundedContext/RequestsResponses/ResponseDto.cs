using System.Net;
using WeeControl.Common.BoundedContext.Interfaces;

namespace WeeControl.Common.BoundedContext.RequestsResponses
{
    public class ResponseDto
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