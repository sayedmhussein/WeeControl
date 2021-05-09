using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.ClientService.Interfaces;

namespace MySystem.ClientService.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private IAppSettings AppSettings => Ioc.Default.GetService<IAppSettings>();
        private IDeviceAction DeviceAction => Ioc.Default.GetService<IDeviceAction>();

        public string WelComeMessage
        {
            get
            {
                return "Hello User";
            }
        }

        public string Disclaimer { get => AppSettings.HomeDisclaimer; }

        public ICommand OpenWebCommand { get; }

        public HomeViewModel()
        {
            OpenWebCommand = new RelayCommand(async () => await DeviceAction.OpenWebPageAsync("http://www.google.com/"));
        }
    }
}
