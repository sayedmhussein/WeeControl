using System;
using System.Collections.Immutable;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.Applications.BaseLib.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        #region Private Properties
        private readonly HttpClient httpClient;
        private readonly IDevice device;
        private readonly ImmutableDictionary<ApiRouteEnum, string> routes;
        #endregion

        #region Public Properties
        public string NameOfUser => device.FullUserName;

        #endregion

        #region Commands
        public ICommand HelpCommand { get; }
        public ICommand LogoutCommand { get; }
        #endregion

        #region Constructors
        public ShellViewModel() : this(Ioc.Default.GetRequiredService<IViewModelDependencyFactory>(), Ioc.Default.GetRequiredService<IApiDicts>())
        {
        }

        public ShellViewModel(IViewModelDependencyFactory service, IApiDicts commonValues)
        {
            if (service == null || commonValues == null)
            {
                throw new ArgumentNullException();
            }

            httpClient = service.HttpClientInstance;
            device = service.Device;
            routes = commonValues.ApiRoute;

            HelpCommand = new RelayCommand(async () => await device.OpenWebPageAsync("http://www.google.com/"));
            LogoutCommand = new AsyncRelayCommand(Logout);
        }
        #endregion

        #region Private Functions
        private async Task Logout()
        {
            device.Token = string.Empty;
            await Task.Run(async () =>
            {
                try
                {
                    device.Token = string.Empty;
                    await httpClient.DeleteAsync(routes[ApiRouteEnum.EmployeeSession]);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await device.TerminateAppAsync();
                }
            });
        }
        #endregion
    }
}
