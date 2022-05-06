using WeeControl.Frontend.ServiceLibrary.Enums;

namespace WeeControl.Frontend.ServiceLibrary.BoundedContexts.Authorization.UiResponsObjects;

public class LoginResponse
{
    public bool IsSuccess { get; set; } = false;

    public string MessageToUser { get; set; } = string.Empty;
    
    public static LoginResponse Success()
    {
        return new LoginResponse() { IsSuccess = true};
    }

    public static LoginResponse Failed(string messageToUser)
    {
        return new LoginResponse() { MessageToUser = messageToUser};
    }
}