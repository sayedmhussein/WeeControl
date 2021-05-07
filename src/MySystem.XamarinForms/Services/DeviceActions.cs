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
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, message, "bla");
        }

        public object GetRequestDto<T>(T payload)
        {
            return new ResponseDto<T>(payload);
        }

        public Task NavigateAsync(string pageName)
        {
            throw new NotImplementedException();
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
    }
}
