using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.ClientService.Interfaces;
using MySystem.SharedDto.V1;
using MySystem.SharedDto.V1.Custom;

namespace MySystem.ClientService.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private IAppSettings AppSetting => Ioc.Default.GetRequiredService<IAppSettings>();
        private IDeviceInfo DeviceInfo => Ioc.Default.GetRequiredService<IDeviceInfo>();
        private IDeviceAction DeviceAction => Ioc.Default.GetRequiredService<IDeviceAction>();

        private bool isLoading;
        public bool IsLoading
        {
            get => IsLoading;
            set => SetProperty(ref isLoading, value);
        }
        public bool IsNotLoading { get => !isLoading; }

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

        public string Instructions { get => AppSetting.LoginDisclaimer; }

        public IAsyncRelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            IsLoading = false;
        }

        private async Task LoginAsync()
        {
            IsLoading = true;

            if (DeviceInfo.InternetIsAvailable)
            {
                try
                {
                    var dto = new RequestDto<LoginDto>(new LoginDto() { Username = Username, Password = Password }) { DeviceId = DeviceInfo.DeviceId };
                    var response = await DeviceInfo.HttpClient.PostAsJsonAsync("/Api/Credentials/login", dto);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsAsync<ResponseDto<string>>();
                        await DeviceInfo.UpdateTokenAsync(data.Payload);
                        await DeviceAction.NavigateToPageAsync("HomePage");
                    }
                    else
                    {
                        
                        await DeviceAction.DisplayMessageAsync("Invalid Credentials", "Invalid uername or password, please try again later.");
                    }
                }
                catch (Exception e)
                {
                    await DeviceAction.DisplayMessageAsync("Exception", e.Message);
                    throw;
                }
            }
            else
            {
                await DeviceAction.DisplayMessageAsync(IDeviceAction.Message.NoInternet);
            }

            IsLoading = false;
        }
    }
}
