using WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Contexts.Essential.Interfaces;

public interface IAuthorizationService : IViewModelBase
{
    Task<bool> IsAuthorized();
    Task Login(LoginModel loginModel);
    Task Logout();
    
}