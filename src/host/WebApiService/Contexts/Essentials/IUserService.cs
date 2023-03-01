using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IUserService
{
    Task AddCustomer(UserProfileUpdateDto dto);
    Task AddEmployee(UserProfileDto dto);
    
    
    
    Task<UserModel> GetUser();
    Task EditUser(object dto);
    Task ChangePassword(UserPasswordChangeRequestDto dto);
}