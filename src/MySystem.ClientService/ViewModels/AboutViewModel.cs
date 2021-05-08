using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.ClientService.Interfaces;

namespace MySystem.ClientService.ViewModels
{
    public class AboutViewModel : ObservableObject
    {
        private IDeviceActions DeviceAction => Ioc.Default.GetService<IDeviceActions>();

        public ICommand OpenWebCommand { get; }

        public AboutViewModel()
        {
            OpenWebCommand = new RelayCommand(async () => await DeviceAction.OpenWebPageAsync("http://www.google.com/"));
        }
    }
}
