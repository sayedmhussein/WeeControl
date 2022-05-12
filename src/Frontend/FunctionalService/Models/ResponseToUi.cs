using System.Net;
using WeeControl.Frontend.FunctionalService.Interfaces;

namespace WeeControl.Frontend.FunctionalService.Models;

public class ResponseToUi : IResponseToUi
{
    public bool IsSuccess { get; private set; }

    public string MessageToUser { get; private set; }

    public HttpStatusCode HttpStatusCode { get; private set; }

    public static IResponseToUi Accepted(HttpStatusCode code)
    {
        return new ResponseToUi() { IsSuccess = true, MessageToUser = string.Empty, HttpStatusCode = code};
    }

    public static IResponseToUi Rejected(HttpStatusCode code, string messageToUser)
    {
        return new ResponseToUi() { IsSuccess = false, MessageToUser = messageToUser, HttpStatusCode = code };
    }
    
    private ResponseToUi()
    {
    }
}