using WeeControl.Common.SharedKernel.DataTransferObjects.Essential.User;
using WeeControl.Frontend.FunctionalService.Interfaces;

namespace WeeControl.Frontend.FunctionalService.EssentialContext
{
    public interface IUserOperation
    {
        Task<IResponseToUi> RegisterAsync(RegisterDto loginDto);
        Task<IResponseToUi> LoginAsync(LoginDto loginDto);
        Task<IResponseToUi> GetTokenAsync();
        Task<IResponseToUi> LogoutAsync();
        Task<IResponseToUi> UpdatePasswordAsync(UpdatePasswordDto loginDto);
        Task<IResponseToUi> UpdateEmailAsync(UpdateEmailAsync loginDto);
        Task<IResponseToUi> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    }
}