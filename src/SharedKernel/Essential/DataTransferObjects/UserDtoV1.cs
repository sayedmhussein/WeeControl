namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class UserDtoV1
{
    public static UserDtoV1 Create(string email, string username, string territory)
    {
        return new UserDtoV1() { Email = email, Username = username,TerritoryCode = territory };
    }

    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string TerritoryCode { get; set; } = string.Empty;
    
    
}