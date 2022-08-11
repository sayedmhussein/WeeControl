namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class AuthenticationRequestDto
{
    public string UsernameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public static AuthenticationRequestDto Create(string usernameOrEmail, string password)
    {
        return new AuthenticationRequestDto()
        {
            UsernameOrEmail = usernameOrEmail, Password = password
        };
    }
}