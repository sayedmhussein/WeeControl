using System;
using System.Threading.Tasks;
using MySystem.ClientService.Interfaces;
using MySystem.SharedDto.V1;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MySystem.XamarinForms.Services
{
    public class DeviceActions : IDeviceActions
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

        public async Task NavigateAsync(string pageName)
        {
            await Shell.Current.GoToAsync($"//{pageName}");
        }

        public async Task OpenWebPageAsync(string url)
        {
            await Browser.OpenAsync(url);
        }

        public void PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        public void TerminateApp()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
    }
}
