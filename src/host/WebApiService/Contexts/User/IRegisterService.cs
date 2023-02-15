using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Host.WebApiService.Contexts.User;

public interface IRegisterService
{
    Task Register(EmployeeRegisterDto dto);
    Task<UserModel> GetUser();
    Task EditUser(object dto);
    Task ChangePassword(UserPasswordChangeRequestDto dto);
    Task RequestPasswordReset(UserPasswordResetRequestDto dto);
}