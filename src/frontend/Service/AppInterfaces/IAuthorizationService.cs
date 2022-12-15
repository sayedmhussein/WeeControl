

namespace WeeControl.Frontend.AppService.AppInterfaces;

public interface IAuthorizationService
{
    [Obsolete("To be removed from authorization class!")]
    Task<bool> IsAuthorized();
    Task<bool> Login(string usernameOrEmail, string password);
    Task<bool> UpdateToken(string otp);
    Task<bool> Logout();
}