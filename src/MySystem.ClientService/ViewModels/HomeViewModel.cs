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
        private readonly IDevice device;
        private readonly IClientServices service ;

        #region Public Properties
        public string WelComeMessage
        {
            get
            {
                return "Hello " + device.FullUserName;
            }
        }

        public string Disclaimer { get => service.Settings.Home.Text; }
        #endregion

        #region Commands
        public ICommand OpenWebCommand { get; }
        #endregion

        #region Constructors
        public HomeViewModel() : this(Ioc.Default.GetService<IDevice>(), Ioc.Default.GetService<IClientServices>())
        {
        }

        public HomeViewModel(IDevice device, IClientServices service)
        {
            this.device = device;
            this.service = service;

            OpenWebCommand = new RelayCommand(async () => await device.OpenWebPageAsync("http://www.google.com/"));
        }
        #endregion
    }
}
