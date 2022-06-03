namespace WeeControl.SharedKernel.DataTransferObjects.User;

public class RegisterDtoV1
{
    public static RegisterDtoV1? Create(string email, string username, string password)
    {
        return new RegisterDtoV1() { Email = email, Username = username, Password = password };
    }

    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}