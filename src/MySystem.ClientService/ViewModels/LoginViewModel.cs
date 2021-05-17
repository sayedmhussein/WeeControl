using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.Shared.Dtos.V1;
using Sayed.MySystem.Shared.Dtos.V1.Custom;

namespace Sayed.MySystem.ClientService.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        #region Private Properties
        private readonly IDevice device;
        private readonly IClientServices service;
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
        public LoginViewModel() : this(Ioc.Default.GetService<IDevice>(), Ioc.Default.GetRequiredService<IClientServices>())
        {
        }

        public LoginViewModel(IDevice device, IClientServices service)
        {
            this.device = device;
            this.service = service;

            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }
        #endregion

        #region Private Functions
        private async Task LoginAsync()
        {
            if (device.Internet)
            {
                VerifyUserInputs();

                try
                {
                    var dto = new RequestDto<LoginDto>(new LoginDto() { Username = Username, Password = Password }) { DeviceId = device.DeviceId };
                    var response = await service.HttpClientInstance.PostAsJsonAsync("/Api/Credentials/login", dto);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsAsync<ResponseDto<string>>();
                        device.Token = data.Payload;
                        await device.NavigateToPageAsync("HomePage");
                    }
                    else
                    {
                        await device.DisplayMessageAsync("Invalid Credentials", "Invalid uername or password, please try again later.");
                    }
                }
                catch (Exception e)
                {
                    await device.DisplayMessageAsync("Exception", e.Message);
                    throw;
                }
            }
            else
            {
                await device.DisplayMessageAsync(IDevice.Message.NoInternet);
            }
        }

        private void VerifyUserInputs()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentNullException("Invalid Username or Password");
            }
        }
        #endregion
    }
}
