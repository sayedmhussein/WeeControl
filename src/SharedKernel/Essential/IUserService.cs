using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.SharedKernel.Essential;

public interface IUserService
{
    Task RegisterAsync(RegisterDtoV1 loginDtoV1);
    Task LoginAsync(LoginDtoV1 loginDtoV1);
    Task GetTokenAsync();
    Task LogoutAsync();
    Task UpdatePasswordAsync(MeForgotPasswordDtoV1 loginDtoV1);
    Task ForgotPasswordAsync(PutNewPasswordDtoV1 putNewPasswordDtoV1);
}