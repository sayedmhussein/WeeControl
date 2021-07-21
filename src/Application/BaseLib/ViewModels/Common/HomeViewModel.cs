using System.Net.Http;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common;

namespace WeeControl.Applications.BaseLib.ViewModels.Common
{
    public class HomeViewModel : ObservableObject
    {
        private readonly IDevice device;
        private readonly HttpClient httpClient;
        private readonly ICommonLists commonValues;

        #region Public Properties
        public string WelComeMessage
        {
            get
            {
                return "Hello " + device?.FullUserName;
            }
        }

        public string Disclaimer { get => "Disclaimer"; }// dependencyFactory.Settings.Home.Text; }
        #endregion

        #region Commands
        public ICommand RefreshTokenCommand { get; }
        public ICommand OpenWebCommand { get; }
        #endregion

        #region Constructors
        public HomeViewModel() : this(Ioc.Default.GetService<IDevice>(), Ioc.Default.GetRequiredService<ICommonLists>())
        {
        }

        public HomeViewModel(IDevice device, ICommonLists commonValues)
        {
            this.device = device;
            httpClient = device.HttpClientInstance;
            this.commonValues = commonValues;

            //RefreshTokenCommand = new AsyncRelayCommand(SyncWithServer);
            OpenWebCommand = new RelayCommand(async () => await device.OpenWebPageAsync("http://www.google.com/"));
        }
        #endregion

        //private async Task SyncWithServer()
        //{
        //    if (device.Internet)
        //    {
        //        var dto = new RefreshLoginDto()
        //        {
        //            Metadata = (RequestMetadata)await device.GetMetadataAsync(false)
        //        };

        //        try
        //        {
        //            var route = commonValues.ApiRoute[ApiRouteEnum.EmployeeSession];
        //            var response = await httpClient.PutAsJsonAsync(route, dto);

        //            switch (response.StatusCode)
        //            {
        //                case System.Net.HttpStatusCode.OK:
        //                    var responseDto = await response.Content.ReadAsAsync<EmployeeTokenDto>();
        //                    device.Token = responseDto?.Token;
        //                    device.FullUserName = responseDto?.FullName;
        //                    break;
        //                default:
        //                    device.Token = string.Empty;
        //                    await device.OpenWebPageAsync("//LoginPage");
        //                    break;
        //            }
        //        }
        //        catch
        //        { }
        //    }
        //}
    }
}
