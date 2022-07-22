using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class UserModel : IUserModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public string TerritoryId { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
}