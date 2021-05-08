using System.Net.Http;
using MySystem.ClientService.Interfaces;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using MySystem.SharedDto.V1;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Text;
using Newtonsoft.Json;
using System;

namespace MySystem.ClientService.ViewModels
{
    public class SplashViewModel : ObservableObject
    {
        private IAppSettings AppSettings => Ioc.Default.GetService<IAppSettings>();
        private IDeviceInfo DeviceInfo => Ioc.Default.GetRequiredService<IDeviceInfo>();

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
                DeviceAction.TerminateApp();
            }

            try
            {
                var client = await DeviceResources.GetHttpClientAsync();
                var response = await client.PostAsJsonAsync(ApiUri.RefreshToken, DeviceAction.GetRequestDto(new object()));
                if (response.IsSuccessStatusCode)
                {
                    await DeviceAction.NavigateAsync("MainPage");
                }
                else
                {
                    await DeviceAction.NavigateAsync("LoginPage");
                }
            }
            catch(Exception e)
            {
                await DeviceAction.DisplayMessageAsync("Issue", "Something Unexpected Occured!");
                DeviceAction.TerminateApp();
                throw;
            }
        }
    }
}
