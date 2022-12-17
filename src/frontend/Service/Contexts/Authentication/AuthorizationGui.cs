using System.Globalization;

namespace WeeControl.Frontend.AppService.Contexts.Authentication;

public class AuthorizationGui
{
    public string UsernameLabel => "Username";
    public string PasswordLabel => "Password";
    public string LoginButtonLabel => "Login";
    
    
    public string InvalidUsernameMessage => "Please enter valid username.";
    public string InvalidPasswordMessage => "Please enter valid username.";
    public string InvalidOtpMessage => "Please enter valid OTP.";

    public string UnmatchedUsernameAndPassword => "Either username or password are not correct.";
    public string UserIsBlocked => "Account suspended, please contact the admin.";
    
    public string ApplicationError => "Internal App Error.";
}