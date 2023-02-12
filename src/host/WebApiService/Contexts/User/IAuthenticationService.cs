using WeeControl.Core.DataTransferObject.Contexts.User;

namespace WeeControl.Host.WebApiService.Contexts.User;

public interface IAuthenticationService
{
    Task Login(LoginRequestDto dto);
    Task UpdateToken();
    Task UpdateToken(string otp);
    Task Logout();
}