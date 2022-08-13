using WeeControl.Frontend.ApplicationService.Anonymous.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Anonymous.Interfaces;

public interface IAuthorizationViewModel : IViewModelBase
{
    public LoginModel LoginModel { get; }
   

    Task Init();
    Task Login();
    Task Logout();
    
}