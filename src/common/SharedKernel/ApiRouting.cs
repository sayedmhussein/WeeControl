

namespace WeeControl.Common.SharedKernel;

public class ApiRouting
{
    private const string PreName = "Api/";
    
    public const string AuthorizationRoute = PreName +  "Authorization";
    
    public const string UserRoute = PreName +  "User";
    public const string UserHomeEndpoint = "Home";
    public const string UserPasswordEndpoint = "Password";
    public const string UserNotificationEndpoint = "Notification";

}