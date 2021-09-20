using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1
{
    public class ResponseDto : ISerializable, IVerifyable, IResponseDto
    {
        public HttpStatusCode HttpStatuesCode { get; set; }
    }
}