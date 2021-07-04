using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using WeeControl.Applications.BaseLib.Interfaces;

namespace WeeControl.Applications.BaseLib.ViewModels.Common
{
    public class AboutViewModel : ObservableObject
    {
        private readonly IDevice device;

        #region Commands
        public IAsyncRelayCommand OpenWebCommand { get; }
        #endregion

        #region Constructors
        public AboutViewModel() : this(Ioc.Default.GetService<IDevice>())
        {
        }

        public AboutViewModel(IDevice service)
        {
            device = service;

            OpenWebCommand = new AsyncRelayCommand(async () => await device.OpenWebPageAsync("https://github.com/sayedmhussein/WeeControl"));
        }
        #endregion
    }
}
