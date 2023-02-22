using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials.Services;

internal class RegisterService : IRegisterService
{
    public Task RegisterCustomer(CustomerRegisterDto dto)
    {
        throw new NotImplementedException();
    }

    public Task Register(EmployeeRegisterDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> GetUser()
    {
        throw new NotImplementedException();
    }

    public Task Login(LoginRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }

    public Task EditUser(object dto)
    {
        throw new NotImplementedException();
    }

    public Task ChangePassword(UserPasswordChangeRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task RequestPasswordReset(UserPasswordResetRequestDto dto)
    {
        throw new NotImplementedException();
    }
}