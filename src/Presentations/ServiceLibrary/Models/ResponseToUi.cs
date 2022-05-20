using System.Net;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Presentations.ServiceLibrary.Models;

[Obsolete("To use different methods.")]
public class ResponseToUi : IResponseDto
{
    public bool IsSuccess { get; private set; }

    public HttpStatusCode HttpStatusCode { get; private set; }

    public static IResponseDto Accepted(HttpStatusCode code)
    {
        return new ResponseToUi() { IsSuccess = true, HttpStatusCode = code};
    }

    public static IResponseDto Rejected(HttpStatusCode code, string messageToUser)
    {
        return new ResponseToUi() { IsSuccess = false, HttpStatusCode = code };
    }
    
    private ResponseToUi()
    {
    }
}