using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class UserRegisterDto : IUserModel
{
    public static UserRegisterDto Create(IUserModel model)
    {
        return new UserRegisterDto()
        {
            FirstName = model.FirstName,
            
            LastName = model.LastName,
            Email = model.Email,
            Username = model.Username,
            Password = model.Password,
            MobileNo = model.MobileNo,
            TerritoryId = model.TerritoryId,
            Nationality = model.Nationality
        };
    }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public string TerritoryId { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
}