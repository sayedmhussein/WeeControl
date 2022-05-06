using System.Net;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Frontend.ServiceLibrary.BoundedContexts.Authorization.UiResponsObjects;

namespace WeeControl.Frontend.ServiceLibrary.BoundedContexts.Authorization
{
    public interface IUserOperation
    {
        Task RegisterAsync(RegisterDto loginDto);
        Task<LoginResponse> LoginAsync(LoginDto loginDto);
        Task GetTokenAsync();
        Task LogoutAsync();
        Task UpdateEmailAsync(UpdateEmailAsync loginDto);
        Task UpdatePasswordAsync(UpdatePasswordDto loginDto);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    }
}