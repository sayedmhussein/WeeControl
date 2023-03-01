using WeeControl.Core.DataTransferObject.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IAuthenticationService
{
    Task Login(LoginRequestDto dto);
    Task UpdateToken();
    Task UpdateToken(string otp);
    Task Logout();
    
    Task RequestPasswordReset(UserPasswordResetRequestDto dto);
}