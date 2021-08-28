using System;
namespace ClientLib.Services.Employee
{
    public interface IEmployeeService
    {
        string GetToken(string username, string password);

        string RefreshToken();
    }
}
