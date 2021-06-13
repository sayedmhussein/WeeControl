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
using MySystem.SharedKernel.ExtensionMethods;
using MySystem.User.Employee.Services;

namespace MySystem.User.Employee.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        #region Private Properties
        private readonly IDevice device;
        private readonly IClientServices service;
        private readonly ILogger logger;
        private readonly RequestMetadata metadata;
        #endregion

        #region Public Properties
        private string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string Instructions { get => service.Settings.Login.Disclaimer; }
        #endregion

        #region Commands
        public IAsyncRelayCommand LoginCommand { get; }
        #endregion

        #region Constructors
        public LoginViewModel()
            : this(Ioc.Default.GetRequiredService<IClientServices>())
        {
        }

        public LoginViewModel(IClientServices service)
        {
            this.service = service ?? throw new ArgumentNullException();
            this.device = service.Device;
            this.logger = service.Logger;
            this.metadata = (RequestMetadata)device.Metadata;

            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }
        #endregion

        #region Private Functions
        private async Task LoginAsync()
        {
            if (device.Internet)
            {
                var loginDto = new CreateLoginDto() { Username = Username, Password = Password, Metadata = metadata };
                if (loginDto.IsValid() == false)
                {
                    throw new ArgumentException("Invalid Username or Password");
                }

                await CommunicateWithServer(loginDto);
            }
            else
            {
                await device.DisplayMessageAsync(IDevice.Message.NoInternet);
            }
        }

        private async Task CommunicateWithServer(CreateLoginDto dto)
        {
            HttpResponseMessage response;
            try
            {
                response = await service.HttpClientInstance.PostAsJsonAsync(
                    service.SharedValues.ApiRoute[ApiRouteEnum.EmployeeSession], dto);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var data = await response.Content.ReadAsAsync<EmployeeTokenDto>();
                        device.Token = data.Token;
                        await device.NavigateToPageAsync("HomePage");
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        await device.DisplayMessageAsync("Invalid Credentials", service.Settings.Login.InvalidCredentialsMessage);
                        break;
                    default:
                        break;
                }
            }
            catch (System.Net.WebException)
            {
                //logger?.LogWarning("Server Issue! {0}/{1} - V{2}.", service.SharedValues.Base, service.SharedValues.Token, service.SharedValues.Version);
                await device.DisplayMessageAsync("Server Connection Error", "The Application can't connect to the server, ensure that the applicaiton is updated or try again later.");
                return;
            }
            catch (Exception e)
            {
                logger?.LogWarning(e.StackTrace);
#if DEBUG
                await device.DisplayMessageAsync("Unexpected Error Occured!", e.Message);
                throw;
#endif
            }
        }
        #endregion
    }
}
