using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sayed.MySystem.Api.FunctionalTest.Operations
{
    public class Authentication
    {
        private readonly HttpClient client;

        public Authentication(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> LoginAsync(string username, string password, string device)
        {
            throw new NotImplementedException();
        }
    }
}
