using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.BoundedContexts.Shared
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