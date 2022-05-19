using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Essential;

public interface IUserOperation
{
    Task<IResponseDto> RegisterAsync(RegisterDto loginDto);
    Task<IResponseDto> LoginAsync(LoginDto loginDto);
    Task<IResponseDto> GetTokenAsync();
    Task<IResponseDto> LogoutAsync();
    Task<IResponseDto> UpdatePasswordAsync(PasswordSetForgottenDto loginSetForgottenDto);
    Task<IResponseDto> ForgotPasswordAsync(PasswordResetRequestDto passwordResetRequestDto);
}