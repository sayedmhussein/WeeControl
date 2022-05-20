using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Essential;

public interface IUserOperation
{
    Task<IResponseDto> RegisterAsync(RegisterDtoV1 loginDtoV1);
    Task<IResponseDto> LoginAsync(LoginDtoV1 loginDtoV1);
    Task<IResponseDto> GetTokenAsync();
    Task<IResponseDto> LogoutAsync();
    Task<IResponseDto> UpdatePasswordAsync(MeForgotPasswordDtoV1 loginDtoV1);
    Task<IResponseDto> ForgotPasswordAsync(PutNewPasswordDtoV1 putNewPasswordDtoV1);
}