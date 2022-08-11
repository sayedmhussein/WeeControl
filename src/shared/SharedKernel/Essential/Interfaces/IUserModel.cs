namespace WeeControl.SharedKernel.Essential.Interfaces;

[Obsolete]
public interface IUserModel
{
    string FirstName { get; set; }
    public string SecondName { get; set; }
    public string ThirdName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
    string Username { get; set; }
    string Password { get; set; }
    string MobileNo { get; set; }
    string TerritoryId { get; set; }
    string Nationality { get; set; }
}