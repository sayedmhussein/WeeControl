
namespace WeeControl.SharedKernel.DataTransferObjects.Authentication;

public class LoginDtoV1
{
    public static LoginDtoV1? Create(string usernameOrEmail, string password)
    {
        return new LoginDtoV1() { UsernameOrEmail = usernameOrEmail, Password = password };
    }
    
    public string UsernameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}