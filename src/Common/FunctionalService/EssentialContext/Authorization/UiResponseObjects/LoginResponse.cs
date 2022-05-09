using System.Net;
using WeeControl.Common.FunctionalService.Interfaces;

namespace WeeControl.Common.FunctionalService.EssentialContext.Authorization.UiResponseObjects;

public class LoginResponse : IResponseToUi
{
    public bool IsSuccess { get; private set; } = false;

    public string MessageToUser { get; private set; } = string.Empty;

    public HttpStatusCode HttpStatusCode { get; private set; }
    
    public static LoginResponse Accepted(HttpStatusCode code = HttpStatusCode.Accepted)
    {
        return new LoginResponse() { IsSuccess = true, HttpStatusCode = code};
    }

    public static LoginResponse Rejected(HttpStatusCode code, string messageToUser)
    {
        return new LoginResponse() { MessageToUser = messageToUser, HttpStatusCode = code };
    }

    private LoginResponse()
    {
    }
}