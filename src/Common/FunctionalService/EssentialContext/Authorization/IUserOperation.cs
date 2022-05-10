using System.Threading.Tasks;
using WeeControl.Common.FunctionalService.EssentialContext.Authorization.UiResponseObjects;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;

namespace WeeControl.Common.FunctionalService.EssentialContext.Authorization
{
    public interface IUserOperation
    {
        Task RegisterAsync(RegisterDto loginDto);
        Task<LoginResponse> LoginAsync(LoginDto loginDto);
        Task GetTokenAsync();
        Task<LogoutResponse> LogoutAsync();
        Task UpdateEmailAsync(UpdateEmailAsync loginDto);
        Task UpdatePasswordAsync(UpdatePasswordDto loginDto);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    }
}