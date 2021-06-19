using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using WeeControl.Applications.BaseLib.Interfaces;

namespace WeeControl.Applications.BaseLib.ViewModels
{
    public class AboutViewModel : ObservableObject
    {
        private readonly IDevice device;

        #region Commands
        public ICommand OpenWebCommand { get; }
        #endregion

        #region Constructors
        public AboutViewModel() : this(Ioc.Default.GetService<IViewModelDependencyFactory>())
        {
        }

        public AboutViewModel(IViewModelDependencyFactory service)
        {
            device = service.Device;

            OpenWebCommand = new RelayCommand(async () => await device.OpenWebPageAsync("https://github.com/sayedmhussein/WeeControl"));
        }
        #endregion
    }
}
