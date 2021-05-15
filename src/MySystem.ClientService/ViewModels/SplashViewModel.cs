using System.Net.Http;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.Shared.Dtos.V1;

namespace Sayed.MySystem.ClientService.ViewModels
{
    public class SplashViewModel : ObservableObject
    {
        private readonly IDevice device;
        private readonly IClientServices service;

        public IAsyncRelayCommand RefreshTokenCommand { get; }

        public SplashViewModel() : this(Ioc.Default.GetService<IDevice>(), Ioc.Default.GetRequiredService<IClientServices>())
        {
        }

        public SplashViewModel(IDevice device, IClientServices client)
        {
            this.device = device;
            this.service = client;
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
                        var response = await service.HttpClient.PostAsJsonAsync(service.Api.Token, dto);
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
                        service.LogAppend(e.Message);
                    }
                });
            }
        }
    }
}
