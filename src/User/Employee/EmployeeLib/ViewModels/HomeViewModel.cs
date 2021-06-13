using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using MySystem.User.Employee.Interfaces;
using MySystem.User.Employee.Services;

namespace MySystem.User.Employee.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private readonly IDevice device;
        private readonly IViewModelDependencyFactory dependencyFactory ;

        #region Public Properties
        public string WelComeMessage
        {
            get
            {
                return "Hello " + device.FullUserName;
            }
        }

        public string Disclaimer { get => "Disclaimer"; }// dependencyFactory.Settings.Home.Text; }
        #endregion

        #region Commands
        public ICommand OpenWebCommand { get; }
        #endregion

        #region Constructors
        public HomeViewModel() : this(Ioc.Default.GetService<IDevice>(), Ioc.Default.GetService<IViewModelDependencyFactory>())
        {
        }

        public HomeViewModel(IDevice device, IViewModelDependencyFactory dependencyFactory)
        {
            this.device = device;
            this.dependencyFactory = dependencyFactory;

            OpenWebCommand = new RelayCommand(async () => await device.OpenWebPageAsync("http://www.google.com/"));
        }
        #endregion
    }
}
