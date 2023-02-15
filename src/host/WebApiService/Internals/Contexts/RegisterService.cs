using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.SharedKernel.Contexts.User;
using WeeControl.Host.WebApiService.Contexts.User;

namespace WeeControl.Host.WebApiService.Internals.Contexts;

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