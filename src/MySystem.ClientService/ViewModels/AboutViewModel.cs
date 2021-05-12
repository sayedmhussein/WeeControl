﻿using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Sayed.MySystem.ClientService.Services;

namespace Sayed.MySystem.ClientService.ViewModels
{
    public class AboutViewModel : ObservableObject
    {
        private IDevice DeviceAction => Ioc.Default.GetService<IDevice>();

        public ICommand OpenWebCommand { get; }

        public AboutViewModel()
        {
            OpenWebCommand = new RelayCommand(async () => await DeviceAction.OpenWebPageAsync("http://www.google.com/"));
        }
    }
}
