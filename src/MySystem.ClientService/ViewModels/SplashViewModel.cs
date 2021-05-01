using System;
using System.Net.Http;
using System.Windows.Input;
using MySystem.ClientService.Interfaces;
using MySystem.ClientService.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using MySystem.SharedDto.V1;

namespace MySystem.ClientService.ViewModels
{
    public class SplashViewModel : ObservableObject
    {
        public IDeviceInfoService DeviceInfo { get; set; }
        public IDeviceActionService DeviceAction { get; }
        public IDeviceResources DeviceResources { get; set; }

        private string splashLabel; 

        public string SplashLabel
        {
            get => splashLabel;
            set => SetProperty(ref splashLabel, value);
        }

        public string AppLogoPath { get; }

        public IAsyncRelayCommand RefreshTokenCommand { get; }

        public SplashViewModel()
        {
            RefreshTokenCommand = new AsyncRelayCommand(VerifyTokenAsync);
            SplashLabel = "Please Wait...";
            AppLogoPath = "MySystem.XamarinForms.Resources.splashlogo.png";
            //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/images?tabs=macos#local-images
            //deviceInfo = App.Current.Services.GetService<ContactsViewModel>();
        }

        private async Task VerifyTokenAsync()
        {
            if (DeviceInfo.InternetAvailable == false)
            {
                await DeviceAction.DisplayMessageAsync("Alert", "Check internet connection then try again.");
            }

            var bla = DeviceInfo.GetRequestDto(new object());
            var response = await DeviceResources.ApiClient.PostAsJsonAsync("http://142.168.194.107:5000/", bla);
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Accepted:
                case System.Net.HttpStatusCode.OK:
                    var _response = await response.Content.ReadAsAsync<ResponseDto<string>>();
                    DeviceInfo.Token = _response.Payload;
                    break;
                default:
                    break;
            }
        }
    }
}
