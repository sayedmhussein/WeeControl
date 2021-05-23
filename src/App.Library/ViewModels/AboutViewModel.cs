using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.Web.ClientService.Services;

namespace MySystem.Web.ClientService.ViewModels
{
    public class AboutViewModel : ObservableObject
    {
        private readonly IDevice device;

        #region Commands
        public ICommand OpenWebCommand { get; }
        #endregion

        #region Constructors
        public AboutViewModel() : this(Ioc.Default.GetService<IDevice>(), null)
        {
        }

        public AboutViewModel(IDevice device, IClientServices service)
        {
            this.device = device;

            OpenWebCommand = new RelayCommand(async () => await device.OpenWebPageAsync("http://www.google.com/"));
        }
        #endregion
    }
}
