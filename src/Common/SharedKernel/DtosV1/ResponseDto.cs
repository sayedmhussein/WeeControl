using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.DtosV1
{
    public class ResponseDto : IResponseDto
    {
        public int HttpStatuesCode { get; set; }
    }
}