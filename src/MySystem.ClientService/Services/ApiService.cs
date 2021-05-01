using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace MySystem.ClientService.Services
{
    public static class ApiService
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient(IConfiguration config)
        {
            ApiClient = new HttpClient();
            //ApiClient.BaseAddress = new Uri("");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
