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
            string baseAdd;
#if DEBUG
            baseAdd = "http://192.168.126.107:5000";
            //baseAdd = Xamarin.Essentials.DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:5001" : "https://localhost:5001";
#else
            baseAdd = Devic;
#endif
            httpClient = new HttpClient
            {

                BaseAddress = new Uri(baseAdd)
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

        public bool TokenIsNull => string.IsNullOrWhiteSpace(SecureStorage.GetAsync("token").Result);

        public string Token => throw new NotImplementedException();

        public async Task UpdateTokenAsync(string token)
        {
            await SecureStorage.SetAsync("token", token ?? string.Empty);
        }
    }
}
