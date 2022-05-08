using System.Net;

namespace WeeControl.Common.FunctionalService.BoundedContexts.Authorization.UiResponsObjects;

public class LoginResponse
{
    public bool IsSuccess { get; set; } = false;

    public string MessageToUser { get; set; } = string.Empty;

    public HttpStatusCode HttpStatusCode { get; set; }
    
    public static LoginResponse Accepted(HttpStatusCode code = HttpStatusCode.Accepted)
    {
        return new LoginResponse() { IsSuccess = true, HttpStatusCode = code};
    }

    public static LoginResponse Rejected(HttpStatusCode code, string messageToUser)
    {
        return new LoginResponse() { MessageToUser = messageToUser, HttpStatusCode = code };
    }
}