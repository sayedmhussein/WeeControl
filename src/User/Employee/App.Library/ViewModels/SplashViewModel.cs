using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.EntityV1Dtos.Employee;
using MySystem.SharedKernel.Enumerators;
using MySystem.User.Employee.Services;

namespace MySystem.User.Employee.ViewModels
{
    public class SplashViewModel : ObservableObject
    {
        private readonly IDevice device;
        private readonly IClientServices service;
        private readonly ILogger logger;
        private readonly RequestMetadata metadata;

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
            this.metadata = (RequestMetadata)device.Metadata;

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

            var dto = new RefreshLoginDto() { Metadata = metadata };

            await CommunicateWithServer(dto);
        }

        private async Task CommunicateWithServer(RefreshLoginDto dto)
        {
            try
            {
                var response = await service.HttpClientInstance.PutAsJsonAsync(service.SharedValues.ApiRoute[ApiRouteEnum.EmployeeSession], dto);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var responseDto = await response.Content.ReadAsAsync<EmployeeTokenDto>();
                        device.Token = responseDto?.Token;
                        await device.NavigateToPageAsync("HomePage");
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        await device.NavigateToPageAsync("LoginPage");
                        break;
                    default:
                        await device.NavigateToPageAsync("LoginPage");
                        break;
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
        }
    }
}
