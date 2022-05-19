using System.Net;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.RequestsResponses;

public class ResponseDto : IResponseDto
{
    public bool IsSuccess { get; }
    
    public HttpStatusCode HttpStatusCode { get; }
        
    protected ResponseDto()
    {
    }
}