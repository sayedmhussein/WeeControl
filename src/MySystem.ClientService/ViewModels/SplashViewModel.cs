using System.Net.Http;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using Sayed.MySystem.SharedDto.V1;
using Sayed.MySystem.ClientService.Services;

namespace Sayed.MySystem.ClientService.ViewModels
{
    public class SplashViewModel : ObservableObject
    {
        private readonly IDevice device;
        private readonly IClientServices client;

        public IAsyncRelayCommand RefreshTokenCommand { get; }

        public SplashViewModel() : this(Ioc.Default.GetService<IDevice>(), Ioc.Default.GetRequiredService<IClientServices>())
        {
        }

        public SplashViewModel(IDevice device, IClientServices client)
        {
            this.device = device;
            this.client = client;
            RefreshTokenCommand = new AsyncRelayCommand(VerifyTokenAsync);
        }

        private async Task VerifyTokenAsync()
        {
            if (device.Internet == false)
            {
                await device.DisplayMessageAsync(IDevice.Message.NoInternet);
                device.TerminateApp();
            }
            else if (string.IsNullOrEmpty(device.Token))
            {
                await device.NavigateToPageAsync("LoginPage");
            }
            else
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        var dto = new RequestDto<object>(device.DeviceId);
                        var response = await client.HttpClient.PostAsJsonAsync(client.Settings.Api.Token, dto);
                        if (response.IsSuccessStatusCode)
                        {
                            var r = await response.Content.ReadAsAsync<ResponseDto<string>>();
                            device.Token = r.Payload;
                            await device.NavigateToPageAsync("HomePage");
                        }
                        else
                        {
                            await device.NavigateToPageAsync("LoginPage");
                        }
                    }
                    catch (System.Net.WebException)
                    {
                        await device.DisplayMessageAsync("Server Connection Error", "The Application can't connect to the server, ensure that the applicaiton is updated or try again later.");
                        device.TerminateApp();
                    }
                    catch (Exception e)
                    {
                        await device.DisplayMessageAsync("Exception", e.Message);
                        device.TerminateApp();
                        throw;
                    }
                });
            }
        }
    }
}
