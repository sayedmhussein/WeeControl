using System;
using System.Net.Http;
using System.Net.Http.Headers;
using MySystem.ClientService.Interfaces;

namespace MySystem.XamarinForms.Services
{
    public class ApiUri : IApiUri
    {
        public string RefreshToken => "/Api/Credentials/token";
    }
}
