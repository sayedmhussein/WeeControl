

namespace WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

public interface IAuthorizationService
{
    Task<bool> Login(string usernameOrEmail, string password);
    Task<bool> UpdateToken(string otp);
    Task<bool> Logout();
    
    public string UsernameLabel { get; } 
    public string PasswordLabel  { get; } 
    public string LoginButtonLabel  { get; } 
    
    public string InvalidUsernameMessage  { get; } 
    public string InvalidPasswordMessage  { get; } 
    public string InvalidOtpMessage  { get; }
    public string UnmatchedUsernameAndPassword  { get; } 
    public string UserIsBlocked  { get; }
    public string ApplicationError  { get; }
}