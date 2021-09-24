using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.BoundedContextDtos.Shared
{
    public class ResponseDto : ISerializable, IVerifyable, IResponseDto
    {
        public HttpStatusCode HttpStatuesCode { get; set; }
    }
}