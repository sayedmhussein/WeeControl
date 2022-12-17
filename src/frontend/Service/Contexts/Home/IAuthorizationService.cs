

using WeeControl.Frontend.AppService.Contexts.Authentication;

namespace WeeControl.Frontend.AppService.Contexts.Home;

public interface IAuthorizationService
{
    AuthorizationGui GuiStrings { get; }
    
    Task<bool> Login(string usernameOrEmail, string password);
    Task<bool> UpdateToken(string otp);
    Task<bool> Logout();
}