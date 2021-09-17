using System.Net;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.DtosV1
{
    public class ResponseDto : IResponseDto
    {
        public HttpStatusCode HttpStatuesCode { get; set; }
    }
}