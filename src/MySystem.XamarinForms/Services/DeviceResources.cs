using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MySystem.ClientService.Interfaces;
using Xamarin.Essentials;

namespace MySystem.XamarinForms.Services
{
    public class DeviceResources : IDeviceResources
    {
        private static HttpClient ApiClient { get; set; }

        public static void InitializeClient(string baseUri, string apiVersion)
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(baseUri);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Add("Accept-version", apiVersion);

            if (SecureStorage.GetAsync("token").Result == null)
            {
                SecureStorage.SetAsync("token", string.Empty).Wait();
            }
        }

        public DeviceResources()
        {
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("token"));
            //ApiClient.Timeout = TimeSpan.FromSeconds(5);
            return ApiClient;
        }

        public async Task SaveTokenAsync(string token)
        {
            await SecureStorage.SetAsync("token", token ?? string.Empty);
        }
    }
}

