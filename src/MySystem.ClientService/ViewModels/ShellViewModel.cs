using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.ClientService.Interfaces;
using MySystem.SharedDto.V1;

namespace MySystem.ClientService.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private IDeviceInfo DeviceInfo => Ioc.Default.GetService<IDeviceInfo>();
        private IDeviceAction DeviceAction => Ioc.Default.GetService<IDeviceAction>();

        public ICommand HelpCommand { get; }
        public ICommand LogoutCommand { get; }

        public ShellViewModel()
        {
            HelpCommand = new RelayCommand(async () => await DeviceAction.OpenWebPageAsync("http://www.google.com/"));
            LogoutCommand = new AsyncRelayCommand(Logout);
        }

        private async Task Logout()
        {
            await DeviceInfo.UpdateTokenAsync(string.Empty);
            await Task.Run(async () =>
            {
                try
                {
                    var dto = new RequestDto<object>(DeviceInfo.DeviceId);
                    var response = await DeviceInfo.HttpClient.PostAsJsonAsync("/Api/Credentials/logout", dto);
                }
                catch
                { }
            });
        }
    }
}
