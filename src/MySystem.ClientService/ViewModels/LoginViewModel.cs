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
        private IDeviceInfo DeviceInfo => Ioc.Default.GetRequiredService<IDeviceInfo>();
        private IDeviceActions DeviceAction => Ioc.Default.GetRequiredService<IDeviceActions>();
        private IDeviceResources DeviceResources => Ioc.Default.GetRequiredService<IDeviceResources>();

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

        public string Instructions { get => "Instructions"; }

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
                    var client = await DeviceResources.GetHttpClientAsync();
                    var dto = new RequestDto<LoginDto>(new LoginDto() { Username = Username, Password = Password }) { DeviceId = DeviceInfo.DeviceId };
                    var response = await client.PostAsJsonAsync("/Api/Credentials/login", dto);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsAsync<ResponseDto<string>>();
                        await DeviceResources.SaveTokenAsync(data.Payload);
                        await DeviceAction.NavigateAsync("AboutPage");
                    }
                    else
                    {
                        await DeviceAction.DisplayMessageAsync("Invalid Credentials", "Invalid uername or password, please try again later.");
                    }
                }
                catch (Exception e)
                {
                    await DeviceAction.DisplayMessageAsync("Issue", "Something Unexpected Occured!");
                    throw;
                }
            }
            else
            {
                await DeviceAction.DisplayMessageAsync("No Internet!", "Check your internet connection then try again later.");
                
            }

            IsLoading = false;
        }
    }
}
