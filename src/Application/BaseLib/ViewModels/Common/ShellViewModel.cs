using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.Services;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Employee;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;

namespace WeeControl.Applications.BaseLib.ViewModels.Common
{
    public class ShellViewModel : ObservableObject
    {
        #region Private Properties
        private readonly IDevice device;
        private readonly IServerService server;
        private readonly IEmployeeLists employeeLists;
        #endregion

        #region Public Properties
        public string NameOfUser => device?.FullUserName ?? "Hello :)";

        public double UpdateProgress => 0.75;
        #endregion

        #region Page Appearence Properties
        private bool isHumanResourcesFlyoutItemVisible = false;
        public bool IsHumanResourcesFlyoutItemVisible
        {
            get => isHumanResourcesFlyoutItemVisible;
            set => SetProperty(ref isHumanResourcesFlyoutItemVisible, value);
        }
        #endregion

        #region Commands
        public IAsyncRelayCommand RefreshCommand { get; }
        public IAsyncRelayCommand HelpCommand { get; }
        public IAsyncRelayCommand LogoutCommand { get; }
        #endregion

        #region Constructors
        public ShellViewModel()
            : this(
                  Ioc.Default.GetRequiredService<IDevice>(),
                  Ioc.Default.GetRequiredService<IServerService>(),
                  Ioc.Default.GetRequiredService<IEmployeeLists>()
                  )
        {
        }

        public ShellViewModel(IDevice device, IServerService server, IEmployeeLists employeeLists)
        {
            this.device = device ?? throw new ArgumentNullException();
            this.server = server ?? throw new ArgumentNullException();
            this.employeeLists = employeeLists ?? throw new ArgumentNullException();

            RefreshCommand = new AsyncRelayCommand(Refresh);
            HelpCommand = new AsyncRelayCommand(async () => await this.device.OpenWebPageAsync("http://www.google.com/"));
            LogoutCommand = new AsyncRelayCommand(Logout);
        }
        #endregion

        #region Private Functions
        private async Task Refresh()
        {
            await server.RefreshTokenAsync();

            var claims = JwtTokenService.GetClaims(await device.GetTokenAsync());
            ScaffoldFlyoutItemsVisibility(claims);
        }

        private async Task Logout()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                Version = new Version("1.0"),
                RequestUri = server.GetUri(ApiRouteEnum.EmployeeSession)
            };

            await server.GetHttpResponseMessageAsync(request, ignoreException: true, displayMessage: false);
            await device.ClearTokenAsync();
            await device.NavigateToPageAsync("//LoginPage");
        }

        private void ScaffoldFlyoutItemsVisibility(IEnumerable<Claim> claims)
        {
            var types = claims?.Select(x => x.Type).ToList();

            IsHumanResourcesFlyoutItemVisible = types.Contains(employeeLists.GetClaimType(ClaimTypeEnum.HumanResources));
        }
        #endregion
    }
}
