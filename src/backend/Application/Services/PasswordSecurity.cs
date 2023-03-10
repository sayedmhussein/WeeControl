using System;
using System.Security.Cryptography;
using System.Text;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.Core.Application.Services;

public class PasswordSecurity : IPasswordSecurity
{
    public string Hash(string password)
    {
        using var md5 = MD5.Create();
        var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
        return Convert.ToBase64String(md5data);
    }

    public bool Verify(string password, string storedPassword)
    {
        return storedPassword == Hash(password);
    }

    public string GenerateRandomPassword(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[length];
        var random = new Random();

        for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

        return new string(stringChars);
    }
}