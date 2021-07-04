using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.Services;

namespace WeeControl.Applications.BaseLib.ViewModels.Common
{
    public class SplashViewModel : ObservableObject
    {
        #region private
        private readonly IDevice device;
        private readonly IServerService server;
        #endregion

        #region Properties
        #endregion

        #region Commands
        public IAsyncRelayCommand RefreshTokenCommand { get; }
        #endregion

        #region Constructors
        public SplashViewModel()
            : this(
                  Ioc.Default.GetRequiredService<IDevice>(),
                  Ioc.Default.GetRequiredService<IServerService>()
                  )
        {
        }

        public SplashViewModel(IDevice device, IServerService server)
        {
            this.device = device ?? throw new ArgumentNullException();
            this.server = server ?? throw new ArgumentNullException();         
            
            RefreshTokenCommand = new AsyncRelayCommand(DoNavigationToPage);
        }
        #endregion

        #region Private Functions
        private async Task DoNavigationToPage()
        {
            var token = await device.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                await device.NavigateToPageAsync("//LoginPage");
            }
            else
            {
                if (device.Internet == false)
                {
                    if (new JwtTokenService(token).IsIssuedBefore(DateTime.UtcNow.AddDays(7)))
                    {
                        await device.NavigateToPageAsync("//HomePage");
                    }
                    else
                    {
                        await device.ClearTokenAsync();
                        await device.NavigateToPageAsync("//LoginPage");
                    }
                }
                else
                {
                    switch (await server.RefreshTokenAsync(ignoreException: true, displayMessage: false))
                    {
                        case System.Net.HttpStatusCode.OK:
                            await device.NavigateToPageAsync("//HomePage");
                            break;
                        default:
                            await device.NavigateToPageAsync("//LoginPage");
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
