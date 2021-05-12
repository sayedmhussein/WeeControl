using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.SharedDto.V1;
using Sayed.MySystem.SharedDto.V1.Custom;

namespace Sayed.MySystem.ClientService.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private IDevice Device => Ioc.Default.GetService<IDevice>();
        private IClientServices ApiClient => Ioc.Default.GetRequiredService<IClientServices>();

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

        public string Instructions { get => ApiClient.Settings.Login.Disclaimer; }

        public IAsyncRelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        private async Task LoginAsync()
        {
            if (Device.Internet)
            {
                try
                {
                    var dto = new RequestDto<LoginDto>(new LoginDto() { Username = Username, Password = Password }) { DeviceId = Device.DeviceId };
                    var response = await ApiClient.HttpClient.PostAsJsonAsync("/Api/Credentials/login", dto);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsAsync<ResponseDto<string>>();
                        Device.Token = data.Payload;
                        await Device.NavigateToPageAsync("HomePage");
                    }
                    else
                    {
                        await Device.DisplayMessageAsync("Invalid Credentials", "Invalid uername or password, please try again later.");
                    }
                }
                catch (Exception e)
                {
                    await Device.DisplayMessageAsync("Exception", e.Message);
                    throw;
                }
            }
            else
            {
                await Device.DisplayMessageAsync(IDevice.Message.NoInternet);
            }
        }
    }
}
