using WeeControl.Frontend.AppService.Contexts.Essential.Models;
using WeeControl.Frontend.AppService.Interfaces;

namespace WeeControl.Frontend.AppService.Contexts.Essential.Interfaces;

public interface IUserAuthorizationService : IViewModelBase
{
    Task<bool> IsAuthorized();
    Task<bool> Login(LoginModel loginModel);
    Task<bool> UpdateToken(string? otp = null);
    Task<bool> Logout();
}