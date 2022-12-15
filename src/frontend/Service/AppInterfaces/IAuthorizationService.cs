

namespace WeeControl.Frontend.AppService.AppInterfaces;

public interface IAuthorizationService
{
    Task<bool> IsAuthorized();
    Task<bool> Login(string usernameOrEmail, string password);
    Task<bool> UpdateToken(string? otp = null);
    Task<bool> Logout();
}