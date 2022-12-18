

namespace WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

public interface IAuthorizationService
{
    Task<bool> Login(string usernameOrEmail, string password);
    Task<bool> UpdateToken(string otp);
    Task<bool> Logout();
    
    public const string UsernameLabel = "Username";
    public const string PasswordLabel = "Password";
    public const string LoginButtonLabel = "Login";
    
    public const string InvalidUsernameMessage = "Please enter valid username.";
    public const string InvalidPasswordMessage = "Please enter valid username.";
    public const string InvalidOtpMessage = "Please enter valid OTP.";
    //
    public const string UnmatchedUsernameAndPassword = "Either username or password are not correct.";
    public const string UserIsBlocked = "Account suspended, please contact the admin.";
    //
    public const string ApplicationError = "Internal App Error.";
}