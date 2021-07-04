using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Common.Extensions;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;

namespace WeeControl.Applications.BaseLib.ViewModels.Common
{
    public class LoginViewModel : ObservableObject
    {
        #region Private
        private readonly IDevice device;
        private readonly Uri requestUri;
        private readonly IServerService server;
        #endregion

        #region Properties
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
            : this(
                  Ioc.Default.GetRequiredService<IDevice>(),
                  Ioc.Default.GetRequiredService<IServerService>()
                  )
        {
        }

        public LoginViewModel(IDevice device, IServerService server)
        {
            if (device == null || server == null)
            {
                throw new ArgumentNullException();
            }

            this.server = server;
            this.device = device;
            requestUri = server.GetUri(ApiRouteEnum.EmployeeSession);

            LoginCommand = new AsyncRelayCommand(DoNavigationToPage);
        }
        #endregion

        #region Private Functions
        private async Task DoNavigationToPage()
        {
            if (device.Internet)
            {
                var request = GetHttpRequestMessage(await device.GetMetadataAsync());
                var response = await server.GetHttpResponseMessageAsync(request, ignoreException: true, displayMessage: true);
                if (response == null)
                { }
                else if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<EmployeeTokenDto>();
                    await device.SaveTokenAsync(data?.Token);
                    await device.NavigateToPageAsync("//HomePage");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await device.DisplayMessageAsync("Invalid Credentials", "Username and Password are not matched.");
                }
                else
                { }
            }
            else
            {
                await device.DisplayMessageAsync(IDeviceAction.Message.NoInternet);
            }
        }

        private HttpRequestMessage GetHttpRequestMessage(IRequestMetadata metadata)
        {
            var dto = new CreateLoginDto()
            {
                Metadata = (RequestMetadata)metadata,
                Username = Username,
                Password = Password
            };

            if (dto.IsValid() == false)
            {
                throw new ArgumentException("Invalid Username or Password");
            }

            return new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Version = new Version("1.0"),
                Content = server.GetHttpContentAsJson(dto),
                RequestUri = requestUri
            };
        }
        #endregion
    }
}
