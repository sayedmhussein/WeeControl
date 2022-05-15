using System.Net;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Presentations.FunctionalService.Models;

public class ResponseToUi : IResponseDto
{
    public bool IsSuccess { get; private set; }

    public string MessageToUser { get; private set; }

    public HttpStatusCode HttpStatusCode { get; private set; }

    public static IResponseDto Accepted(HttpStatusCode code)
    {
        return new ResponseToUi() { IsSuccess = true, MessageToUser = string.Empty, HttpStatusCode = code};
    }

    public static IResponseDto Rejected(HttpStatusCode code, string messageToUser)
    {
        return new ResponseToUi() { IsSuccess = false, MessageToUser = messageToUser, HttpStatusCode = code };
    }
    
    private ResponseToUi()
    {
    }
}