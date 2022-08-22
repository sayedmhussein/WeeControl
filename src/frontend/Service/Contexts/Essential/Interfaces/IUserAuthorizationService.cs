using WeeControl.Frontend.Service.Contexts.Essential.Models;
using WeeControl.Frontend.Service.Interfaces;

namespace WeeControl.Frontend.Service.Contexts.Essential.Interfaces;

public interface IUserAuthorizationService : IViewModelBase
{
    Task<bool> IsAuthorized();
    Task<bool> Login(LoginModel loginModel);
    Task<bool> UpdateToken(string? otp = null);
    Task<bool> Logout();
}