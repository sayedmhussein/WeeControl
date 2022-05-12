using System.Net;
using WeeControl.Frontend.FunctionalService.Interfaces;

namespace WeeControl.Frontend.FunctionalService.EssentialContext.Authorization.UiResponseObjects;

public class LogoutResponse : IResponseToUi
{
    public bool IsSuccess { get; private set; }
    public string MessageToUser { get; private set;}
    public HttpStatusCode HttpStatusCode { get; private set;}
    
    public static LogoutResponse Accepted(HttpStatusCode code = HttpStatusCode.Accepted)
    {
        return new LogoutResponse() { IsSuccess = true, HttpStatusCode = code};
    }

    public static LogoutResponse Rejected(HttpStatusCode code, string messageToUser)
    {
        return new LogoutResponse() { MessageToUser = messageToUser, HttpStatusCode = code };
    }
}