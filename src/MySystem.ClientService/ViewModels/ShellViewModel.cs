using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.SharedDto.V1;

namespace MySystem.ClientService.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private IClientServices ApiClient => Ioc.Default.GetRequiredService<IClientServices>();
        private IDevice DeviceInfo => Ioc.Default.GetService<IDevice>();
        
        public ICommand HelpCommand { get; }
        public ICommand LogoutCommand { get; }

        public ShellViewModel()
        {
            HelpCommand = new RelayCommand(async () => await DeviceInfo.OpenWebPageAsync("http://www.google.com/"));
            LogoutCommand = new AsyncRelayCommand(Logout);
        }

        private async Task Logout()
        {
            DeviceInfo.Token = string.Empty;
            await Task.Run(async () =>
            {
                try
                {
                    var dto = new RequestDto<object>(DeviceInfo.DeviceId);
                    var response = await ApiClient.HttpClient.PostAsJsonAsync("/Api/Credentials/logout", dto);
                }
                catch
                { }
            });
        }
    }
}
