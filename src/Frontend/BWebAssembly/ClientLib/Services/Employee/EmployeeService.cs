using System;
using System.Net.Http;

namespace ClientLib.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient client;
        private readonly IDevice device;

        public EmployeeService(HttpClient client, IDevice device)
        {
            this.client = client;
            this.device = device;
        }

        public string GetToken(string username, string password)
        {
            throw new NotImplementedException();
        }

        public string RefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
