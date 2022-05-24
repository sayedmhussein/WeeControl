namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class ForgotMyPasswordDto
{
    public static ForgotMyPasswordDto Create(string email, string username)
    {
        return new ForgotMyPasswordDto() { Email = email, Username = username };
    }

    public string Username { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    private ForgotMyPasswordDto()
    {
    }
}