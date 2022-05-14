namespace WeeControl.Common.UserSecurityLib.Interfaces;

public interface IPasswordSecurity
{
    string Hash(string password);
    bool Verify(string password, string storedPassword);
}