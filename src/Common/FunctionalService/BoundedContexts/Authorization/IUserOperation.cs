using System.Threading.Tasks;
using WeeControl.Common.FunctionalService.BoundedContexts.Authorization.UiResponsObjects;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;

namespace WeeControl.Common.FunctionalService.BoundedContexts.Authorization
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