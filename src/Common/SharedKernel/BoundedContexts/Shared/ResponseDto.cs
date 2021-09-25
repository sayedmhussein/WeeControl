using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.BoundedContexts.Shared
{
    public class ResponseDto : ISerializable, IVerifyable, IResponseDto
    {
        public HttpStatusCode HttpStatuesCode { get; set; }

        public ResponseDto()
        {
        }

        public ResponseDto(HttpStatusCode httpStatuesCode)
        {
            HttpStatuesCode = httpStatuesCode;
        }
    }
}