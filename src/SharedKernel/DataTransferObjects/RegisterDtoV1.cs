namespace WeeControl.SharedKernel.DataTransferObjects;

public class RegisterDtoV1
{
    public static RegisterDtoV1 Create(string email, string username, string password)
    {
        return new RegisterDtoV1() { Email = email, Username = username, Password = password };
    }
    
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    private RegisterDtoV1()
    {
    }
}