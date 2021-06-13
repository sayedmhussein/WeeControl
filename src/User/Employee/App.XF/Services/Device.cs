﻿using System;
using System.Threading.Tasks;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Interfaces;
using MySystem.User.Employee.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MySystem.User.Employee.XF.Services
{
    public class Device : IDevice
    {
        public IRequestMetadata Metadata => new RequestMetadata()
        {
            Device = Xamarin.Essentials.DeviceInfo.Name
        };

        [Obsolete]
        public string DeviceId => DeviceInfo.Name;

        public string Token
        {
            get => SecureStorage.GetAsync("token").GetAwaiter().GetResult();
            set => SecureStorage.SetAsync("token", value).GetAwaiter().GetResult();
        }

        public bool Internet => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public string FullUserName { get; set; }

        

        public async Task DisplayMessageAsync(string title, string message)
        {
            await DisplayMessageAsync(title, message, "OK");
        }

        public async Task DisplayMessageAsync(string title, string message, string acceptButton)
        {
            //await Shell.Current.DisplayAlert(title, message, acceptButton);
            //await App.Current.MainPage.DisplayAlert(title, message, acceptButton);
            //await Application.Current.MainPage.DisplayAlert(title, message, acceptButton);
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
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return Task.Delay(1);
        }

        public async Task DisplayMessageAsync(IDevice.Message message)
        {
            switch (message)
            {
                case IDevice.Message.NoInternet:
                    await DisplayMessageAsync("No Internet!", "Check your internet connection then try again later.");
                    break;
                default:
                    break;
            }
        }

        public Task PlacePhoneCallAsync(string number)
        {
            throw new NotImplementedException();
        }
    }
}