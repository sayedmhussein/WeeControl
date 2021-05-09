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
        private IDeviceInfo DeviceInfo => Ioc.Default.GetRequiredService<IDeviceInfo>();
        private IDeviceAction DeviceAction => Ioc.Default.GetRequiredService<IDeviceAction>();
        private IApiUri ApiUri => Ioc.Default.GetRequiredService<IApiUri>();

        public IAsyncRelayCommand RefreshTokenCommand { get; }

        public SplashViewModel()
        {
            RefreshTokenCommand = new AsyncRelayCommand(VerifyTokenAsync);
        }

        private async Task VerifyTokenAsync()
        {
            if (DeviceInfo.InternetIsAvailable == false)
            {
                await DeviceAction.DisplayMessageAsync("Alert", "Check internet connection then try again.");
                DeviceAction.TerminateApp();
            }

            try
            {
                var dto = new RequestDto<object>(DeviceInfo.DeviceId);
                var response = await DeviceInfo.HttpClient.PostAsJsonAsync(ApiUri.RefreshToken, dto);
                if (response.IsSuccessStatusCode)
                {
                    await DeviceAction.NavigateToPageAsync("HomePage");
                }
                else
                {
                    await DeviceAction.NavigateToPageAsync("LoginPage");
                }
            }
            catch(Exception e)
            {
                await DeviceAction.DisplayMessageAsync("Exception", e.Message);
                DeviceAction.TerminateApp();
                throw;
            }
        }
    }
}
