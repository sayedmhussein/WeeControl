using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class RegisterDtoV1 : IRegisterDtoV1
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public string TerritoryId { get; set; } = string.Empty;
}