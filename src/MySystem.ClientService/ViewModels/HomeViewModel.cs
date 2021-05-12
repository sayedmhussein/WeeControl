using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Sayed.MySystem.ClientService.Services;

namespace Sayed.MySystem.ClientService.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private Setting AppSettings => Ioc.Default.GetService<Setting>();
        private IDevice DeviceAction => Ioc.Default.GetService<IDevice>();

        public string WelComeMessage
        {
            get
            {
                return "Hello User";
            }
        }

        public string Disclaimer { get => AppSettings.Home.Text; }

        public ICommand OpenWebCommand { get; }

        public HomeViewModel()
        {
            OpenWebCommand = new RelayCommand(async () => await DeviceAction.OpenWebPageAsync("http://www.google.com/"));
        }
    }
}
