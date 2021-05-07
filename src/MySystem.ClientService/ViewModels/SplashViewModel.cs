using System.Net.Http;
using MySystem.ClientService.Interfaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using MySystem.SharedDto.V1;

namespace MySystem.ClientService.ViewModels
{
    public class SplashViewModel : ObservableObject
    {
        public IAppSettings AppSettings { get; set; }
        public IDeviceInfo DeviceInfo { get; set; }
        public IDeviceActions DeviceAction { get; set; }
        public IDeviceResources DeviceResources { get; set; }
        public IApiUri ApiUri { get; set; }

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
            
            AppLogoPath = "MySystem.XamarinForms.Resources.splashlogo.png";
            //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/images?tabs=macos#local-images
            //deviceInfo = App.Current.Services.GetService<ContactsViewModel>();
        }

        private async Task VerifyTokenAsync()
        {
            SplashLabel = AppSettings.WelComeText;

            if (DeviceInfo.InternetIsAvailable == false)
            {
                await DeviceAction.DisplayMessageAsync("Alert", "Check internet connection then try again.");
            }

            var bla = DeviceAction.GetRequestDto(new object());
            var client = await DeviceResources.GetHttpClientAsync();
            var response = await client.PostAsJsonAsync(ApiUri.RefreshToken, bla).ConfigureAwait(false);
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Accepted:
                case System.Net.HttpStatusCode.OK:
                   //var _response = await response.Content.ReadAsStringAsync<ResponseDto<string>>();
                    //await DeviceResources.SaveTokenAsync(_response.Payload);
                    break;
                default:
                    break;
            }
        }
    }
}
