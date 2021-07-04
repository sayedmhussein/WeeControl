using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;

namespace WeeControl.Applications.BaseLib.Services
{
    public class ServerService : IServerService
    {
        private readonly IDevice device;
        private readonly IDeviceCommunication communication;
        private readonly IDeviceStorage storage;
        private readonly IDeviceAction action;
        private readonly IApiDicts apiDicts;

        public ServerService(IDevice device, IDeviceCommunication communication, IDeviceStorage storage, IDeviceAction action, IApiDicts apiDicts)
        {
            this.device = device;
            this.communication = communication;
            this.storage = storage;
            this.action = action;
            this.apiDicts = apiDicts;
        }

        public HttpContent GetHttpContentAsJson(IDto dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        public async Task<HttpResponseMessage> GetHttpResponseMessageAsync(HttpRequestMessage httpRequestMessage, bool ignoreException = false, bool displayMessage = true)
        {
            if (communication.Internet == false)
            {
                if (displayMessage)
                {
                    await action.DisplayMessageAsync(IDeviceAction.Message.NoInternet);
                    return null;
                }
            }

            var token = await storage.GetTokenAsync();
            var client = communication.HttpClientInstance;

            if (string.IsNullOrEmpty(token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            if (ignoreException)
            {
                try
                {
                    return await client.SendAsync(httpRequestMessage);
                }
                catch (System.Net.WebException)
                {
                    if (displayMessage)
                    {
                        await action.DisplayMessageAsync(IDeviceAction.Message.ServerError);
                    }

                    return null;
                }
                catch
                {
                    if (displayMessage)
                    {
                        await action.DisplayMessageAsync(IDeviceAction.Message.InternalError);
                    }

                    return null;
                }
            }
            else
            {
                return await client.SendAsync(httpRequestMessage);
            }
        }

        public Uri GetUri(ApiRouteEnum routeEnum)
        {
            return new Uri(new Uri(apiDicts.ApiRoute[ApiRouteEnum.Base]), apiDicts.ApiRoute[routeEnum]);
        }

        public async Task<HttpStatusCode> RefreshTokenAsync(bool ignoreException = false, bool displayMessage = true)
        {
            var metadata = await device.GetMetadataAsync(exactLocation: true);

            var dto = new RefreshLoginDto()
            {
                Metadata = (RequestMetadata)metadata
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Version = new Version("1.0"),
                Content = GetHttpContentAsJson(dto),
                RequestUri = GetUri(ApiRouteEnum.EmployeeSession)
            };

            var response = await GetHttpResponseMessageAsync(request, ignoreException, displayMessage);
            if (response == null)
            { }
            else if (response.IsSuccessStatusCode)
            {
                var responseDto = await response.Content.ReadAsAsync<EmployeeTokenDto>();
                await device.SaveTokenAsync(responseDto?.Token);
                device.FullUserName = responseDto?.FullName;
            }
            else
            {
                await device.ClearTokenAsync();
                await device.DisplayMessageAsync(IDeviceAction.Message.Logout);
                await device.NavigateToPageAsync("//LoginPage");
            }

            return response.StatusCode;
        }
    }
}
