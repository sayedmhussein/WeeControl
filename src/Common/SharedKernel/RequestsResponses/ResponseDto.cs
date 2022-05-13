using System.Net;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.RequestsResponses;

public class ResponseDto : IResponseDto
{
    public bool IsSuccess { get; }
    public string MessageToUser { get; }
    public HttpStatusCode HttpStatusCode { get; }
        
    protected ResponseDto()
    {
    }
}