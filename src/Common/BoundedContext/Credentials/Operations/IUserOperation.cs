using System.Threading.Tasks;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;

namespace WeeControl.Common.BoundedContext.Credentials.Operations
{
    public interface IUserOperation
    {
        Task RegisterAsync(RegisterDto loginDto);
        Task LoginAsync(LoginDto loginDto);
        Task GetTokenAsync();
        Task LogoutAsync();
        Task UpdateEmailAsync(UpdateEmailAsync loginDto);
        Task UpdatePasswordAsync(UpdatePasswordDto loginDto);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    }
}