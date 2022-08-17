using WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Contexts.Essential.Interfaces;

public interface IUserViewModel : IViewModelBase
{
    CustomerRegisterModel CustomerRegisterModel { get; }
    PasswordResetModel PasswordResetModel { get; }
    PasswordChangeModel PasswordChangeModel { get; }
    IEnumerable<CountryModel> Countries { get; }

    Task<bool> UsernameAllowed();
    Task<bool> EmailAllowed();
    Task<bool> MobileNumberAllowed();
    
    Task Register();
    Task RequestPasswordReset();
    Task ChangeMyPassword();
}