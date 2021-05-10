using System;
using System.Threading.Tasks;
using MySystem.ClientService.Interfaces;
using MySystem.SharedDto.V1;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MySystem.XamarinForms.Services
{
    public class DeviceActions : IDeviceAction
    {
        public DeviceActions()
        {
        }

        public async Task DisplayMessageAsync(string title, string message)
        {
            await DisplayMessageAsync(title, message, "OK");
        }

        public async Task DisplayMessageAsync(string title, string message, string acceptButton)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, acceptButton);
        }

        public async Task NavigateToPageAsync(string pageName)
        {
            await Shell.Current.GoToAsync($"//{pageName}");
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

        public async void PlacePhoneCall(string number)
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

        public void TerminateApp()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }

        public async Task DisplayMessageAsync(IDeviceAction.Message message)
        {
            switch (message)
            {
                case IDeviceAction.Message.NoInternet:
                    await DisplayMessageAsync("No Internet!", "Check your internet connection then try again later.");
                    break;
                default:
                    break;
            }
        }
    }
}
