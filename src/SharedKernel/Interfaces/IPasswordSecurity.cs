namespace WeeControl.SharedKernel.Interfaces;

public interface IPasswordSecurity
{
    string Hash(string password);
    bool Verify(string password, string storedPassword);

    string GenerateRandomPassword(int length = 8);
}