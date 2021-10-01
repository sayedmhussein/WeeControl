using System.Net;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Common.DtosV1
{
    public class ResponseDto : ISerializable, IVerifyable, IResponseDto
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