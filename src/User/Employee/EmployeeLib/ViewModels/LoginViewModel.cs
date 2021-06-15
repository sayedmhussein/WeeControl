using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.EntityV1Dtos.Employee;
using MySystem.SharedKernel.Enumerators.Common;
using MySystem.SharedKernel.ExtensionMethods;
using MySystem.SharedKernel.Interfaces.Values;
using MySystem.User.Employee.Enumerators;
using MySystem.User.Employee.Interfaces;

namespace MySystem.User.Employee.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        #region Private Properties
        private readonly HttpClient httpClient;
        private readonly IDevice device;
        private readonly ICommonValues commonValues;
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

        public string Instructions { get => "Disclaimer"; }// service.Settings.Login.Disclaimer; }
        #endregion

        #region Commands
        public IAsyncRelayCommand LoginCommand { get; }
        #endregion

        #region Constructors
        public LoginViewModel()
            : this(Ioc.Default.GetRequiredService<IViewModelDependencyFactory>(), Ioc.Default.GetRequiredService<ICommonValues>())
        {
        }

        public LoginViewModel(IViewModelDependencyFactory dependencyFactory, ICommonValues commonValues)
        {
            if (dependencyFactory == null || commonValues == null)
            {
                throw new ArgumentNullException();
            }

            this.commonValues = commonValues;
            httpClient = dependencyFactory.HttpClientInstance;
            this.device = dependencyFactory.Device;
            this.logger = dependencyFactory.Logger;
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
                response = await httpClient.PostAsJsonAsync(
                    commonValues.ApiRoute[ApiRouteEnum.EmployeeSession], dto);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var data = await response.Content.ReadAsAsync<EmployeeTokenDto>();
                        device.Token = data.Token;
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(device.Token);
                        await device.NavigateToPageAsync(nameof(ApplicationPageEnum.SplashPage));
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        await device.DisplayMessageAsync("Invalid Credentials", "Username and Password are not matched.");
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
