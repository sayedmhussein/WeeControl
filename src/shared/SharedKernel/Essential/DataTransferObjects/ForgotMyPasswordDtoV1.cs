namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class ForgotMyPasswordDtoV1
{
    public static ForgotMyPasswordDtoV1 Create(string email, string username)
    {
        return new ForgotMyPasswordDtoV1() { Email = email, Username = username };
    }

    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}