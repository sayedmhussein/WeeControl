using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeeControl.Applications.Employee.XF.Services
{
    public class Device : IDevice
    {
        private static HttpClient httpClient;

        public Device()
        {
        }

        public HttpClient HttpClientInstance
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient();
                }
                return httpClient;
            }
        }
        


        public bool Internet => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public string FullUserName { get; set; } = string.Empty;

        public async Task DisplayMessageAsync(string title, string message)
        {
            await DisplayMessageAsync(title, message, "OK");
        }

        public async Task DisplayMessageAsync(string title, string message, string acceptButton)
        {
            await Shell.Current.DisplayAlert(title, message, acceptButton);
            //await App.Current.MainPage.DisplayAlert(title, message, acceptButton);
            //await Application.Current.MainPage.DisplayAlert(title, message, acceptButton);
        }

        public async Task NavigateToPageAsync(string pageName)
        {
            await Shell.Current.GoToAsync(pageName);
        }

        public async Task NavigateToLocationAsync(double latitude, double longitude)
        {
            var location = new Location(latitude, longitude);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            await Map.OpenAsync(location, options);
        }

        public async Task OpenWebPageAsync(string url)
        {
            await Browser.OpenAsync(url);
        }

        public async Task PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException)
            {
                await DisplayMessageAsync("Alert!", "Invalid Mobile Number!");
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayMessageAsync("Alert!","Feature isn't supported on this device.");
            }
            catch
            {
                // Other error has occurred.
            }
        }

        public Task TerminateAppAsync()
        {
            //return NavigateToPageAsync("//LoginPage");
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return Task.CompletedTask;
            //return Task.Delay(1);
        }

        public async Task DisplayMessageAsync(IDevice.Message message)
        {
            switch (message)
            {
                case IDevice.Message.NoInternet:
                    await DisplayMessageAsync("No Internet!", "Check your internet connection then try again later.");
                    break;
                case IDeviceAction.Message.ServerError:
                    await DisplayMessageAsync("Server Error", "The Application can't connect to the server, ensure that the applicaiton is updated or try again later.");
                    break;
                case IDeviceAction.Message.Logout:
                    await DisplayMessageAsync("Logging out!", "Please login again.");
                    break;
                default:
                    await DisplayMessageAsync("Application Unexpected Error!", "We're working on this issue, appreciate your patience.");
                    break;
            }
        }

        public Task PlacePhoneCallAsync(string number)
        {
            throw new NotImplementedException();
        }

        public async Task<IRequestMetadata> GetMetadataAsync(bool exactLocation = false)
        {
            IRequestMetadata metadata = new RequestMetadata();

            try
            {
                Location location;
                if (exactLocation)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                    location = await Geolocation.GetLocationAsync(request, default);
                }
                else
                {
                    location = await Geolocation.GetLastKnownLocationAsync();
                }

                metadata.Device = DeviceInfo.Name;
                metadata.Latitude = location.Latitude;
                metadata.Longitude = location.Longitude;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return metadata;
        }

        public Task SaveTokenAsync(string token)
        {
            return SecureStorage.SetAsync("token", token);
        }

        public Task<string> GetTokenAsync()
        {
            return SecureStorage.GetAsync("token");
        }

        public Task ClearTokenAsync()
        {
            return SecureStorage.SetAsync("token", string.Empty);
        }
    }
}
