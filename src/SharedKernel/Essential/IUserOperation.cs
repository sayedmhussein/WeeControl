using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Essential;

public interface IUserOperation
{
    Task RegisterAsync(RegisterDtoV1 loginDtoV1);
    Task LoginAsync(LoginDtoV1 loginDtoV1);
    Task GetTokenAsync();
    Task LogoutAsync();
    Task UpdatePasswordAsync(MeForgotPasswordDtoV1 loginDtoV1);
    Task ForgotPasswordAsync(PutNewPasswordDtoV1 putNewPasswordDtoV1);
}