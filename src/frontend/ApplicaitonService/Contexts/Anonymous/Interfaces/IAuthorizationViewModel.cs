using WeeControl.Frontend.ApplicationService.Contexts.Anonymous.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Contexts.Anonymous.Interfaces;

public interface IAuthorizationViewModel : IViewModelBase
{
    public LoginModel LoginModel { get; }
    
    Task<bool> IsAuthorized();
    Task Login();
    Task Logout();
    
}