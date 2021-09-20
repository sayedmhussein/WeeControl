using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1
{
    public class ResponseDto : IResponseDto
    {
        public HttpStatusCode HttpStatuesCode { get; set; }
    }
}