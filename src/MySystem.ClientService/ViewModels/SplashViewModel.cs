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
using MySystem.ClientService.Services;

namespace MySystem.ClientService.ViewModels
{
    public class SplashViewModel : ObservableObject
    {
        private IDeviceInfo DeviceInfo => Ioc.Default.GetRequiredService<IDeviceInfo>();
        private IDeviceAction DeviceAction => Ioc.Default.GetRequiredService<IDeviceAction>();

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
            else if (DeviceInfo.TokenIsNull)
            {
                await DeviceAction.NavigateToPageAsync("LoginPage");
            }
            else
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        var dto = new RequestDto<object>(DeviceInfo.DeviceId);
                        var response = await DeviceInfo.HttpClient.PostAsJsonAsync(ApiClient.GetUri(ApiClient.Route.Authentication_Token), dto);
                        if (response.IsSuccessStatusCode)
                        {
                            await DeviceAction.NavigateToPageAsync("HomePage");
                        }
                        else
                        {
                            await DeviceAction.NavigateToPageAsync("LoginPage");
                        }
                    }
                    catch (System.Net.WebException)
                    {
                        await DeviceAction.DisplayMessageAsync("Server Connection Error", "The Application can't connect to the server, ensure that the applicaiton is updated or try again later.");
                        DeviceAction.TerminateApp();
                    }
                    catch (Exception e)
                    {
                        await DeviceAction.DisplayMessageAsync("Exception", e.Message);
                        DeviceAction.TerminateApp();
                        throw;
                    }
                });
            }
        }
    }
}
