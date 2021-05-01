using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MySystem.XamarinForms.Services
{
    public static class ApiService
    {
        public static HttpClient ApiClient { get; set; } 

        public static void InitializeClient(bool IsDevelopment = false)
        {
            ApiClient = new HttpClient();
            //ApiClient.BaseAddress = new Uri("");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
