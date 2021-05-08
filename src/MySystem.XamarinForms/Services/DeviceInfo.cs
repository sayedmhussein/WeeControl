using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MySystem.ClientService.Interfaces;
using MySystem.SharedDto.V1;
using Xamarin.Essentials;

namespace MySystem.XamarinForms.Services
{
    public class DeviceInfo : IDeviceInfo
    {
        private static HttpClient httpClient;
        public HttpClient HttpClient
        {
            get
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SecureStorage.GetAsync("token").Result);
                return httpClient;
            }
        }

        public static void InitializeClient(string baseUri, string apiVersion)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept-version", apiVersion);

            if (SecureStorage.GetAsync("token").Result == null)
            {
                SecureStorage.SetAsync("token", string.Empty).Wait();
            }
        }

        public bool InternetIsAvailable => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public string DeviceId => Xamarin.Essentials.DeviceInfo.Name;

        public async Task UpdateTokenAsync(string token)
        {
            await SecureStorage.SetAsync("token", token ?? string.Empty);
        }
    }
}
