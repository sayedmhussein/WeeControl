using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Host.WebApiService.Contexts.User;

namespace WeeControl.Host.WebApiService.Internals.Contexts;

internal class AuthenticationService : IAuthenticationService
{
    public Task Login(LoginRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateToken()
    {
        throw new NotImplementedException();
    }

    public Task UpdateToken(string otp)
    {
        throw new NotImplementedException();
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }
}