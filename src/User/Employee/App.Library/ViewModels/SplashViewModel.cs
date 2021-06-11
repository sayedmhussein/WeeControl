using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.Persistence.ClientService.Services;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.EntityV1Dtos.Employee;
using MySystem.SharedKernel.Enumerators;

namespace MySystem.Persistence.ClientService.ViewModels
{
    public class SplashViewModel : ObservableObject
    {
        private readonly IDevice device;
        private readonly IClientServices service;
        private readonly ILogger logger;

        public IAsyncRelayCommand RefreshTokenCommand { get; }

        public SplashViewModel()
            : this(Ioc.Default.GetRequiredService<IClientServices>())
        {
        }

        public SplashViewModel(IClientServices client)
        {
            this.service = client ?? throw new ArgumentNullException();
            this.device = service.Device;
            this.logger = service.Logger;

            RefreshTokenCommand = new AsyncRelayCommand(VerifyTokenAsync);
        }

        private async Task VerifyTokenAsync()
        {
            if (string.IsNullOrEmpty(device.Token))
            {
                await device.NavigateToPageAsync("LoginPage");
                return;
            }

            if (device.Internet == false)
            {
                await device.DisplayMessageAsync(IDevice.Message.NoInternet);
                await device.TerminateAppAsync();
                return;
            }

            RequestDto<object> requestDto = new RequestDto<object>(device.DeviceId);
            EmployeeTokenDto responseDto = null;

            try
            {
                var response = await service.HttpClientInstance.PutAsJsonAsync(service.SharedValues.ApiRoute[ApiRouteEnum.EmployeeSession], requestDto);
                if (response.IsSuccessStatusCode)
                {
                    responseDto = await response.Content.ReadAsAsync<EmployeeTokenDto>();
                }
                else
                {
                    await device.NavigateToPageAsync("LoginPage");
                }
            }
            catch (System.Net.WebException)
            {
                //logger?.LogWarning("Server Issue! {0}/{1} - V{2}.", service.SharedValues.Base, service.SharedValues.Token, service.SharedValues.Version);
                await device.DisplayMessageAsync("Server Connection Error", "The Application can't connect to the server, ensure that the applicaiton is updated or try again later.");
                await device.TerminateAppAsync();
                return;
            }
            catch (Exception e)
            {
                logger?.LogCritical(e.Message);
                await device.DisplayMessageAsync("Exception", e.Message);
                await device.TerminateAppAsync();
                return;
            }

            device.Token = responseDto?.Token ?? string.Empty;

            if (device.Token == string.Empty)
            {
                await device.NavigateToPageAsync("LoginPage");
            }
            else
            {
                await device.NavigateToPageAsync("HomePage");
            }
        }
    }
}
