using WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Contexts.Essential.Interfaces;

public interface IUserAuthorizationService : IViewModelBase
{
    Task<bool> IsAuthorized();
    Task Login(LoginModel loginModel);
    Task UpdateToken(string? otp = null);
    Task Logout();
    
}