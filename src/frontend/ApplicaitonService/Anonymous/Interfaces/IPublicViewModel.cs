using WeeControl.Frontend.ApplicationService.Anonymous.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Anonymous.Interfaces;

public interface IPublicViewModel : IViewModelBase
{
    CustomerRegisterModel CustomerRegisterModel { get; }
    PasswordResetModel PasswordResetModel { get; }
    IEnumerable<CountryModel> Countries { get; }

    Task<bool> UsernameAllowed();
    Task<bool> EmailAllowed();
    Task<bool> MobileNumberAllowed();
    
    Task Register();
    Task RequestPasswordReset();
}