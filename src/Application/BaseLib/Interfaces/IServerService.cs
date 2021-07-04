using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IServerService
    {
        Uri GetUri(ApiRouteEnum routeEnum);
        HttpContent GetHttpContentAsJson(IDto dto);
        Task<HttpResponseMessage> GetHttpResponseMessageAsync(HttpRequestMessage httpRequestMessage, bool ignoreException = false, bool displayMessage = true);
        Task<HttpStatusCode> RefreshTokenAsync(bool ignoreException = false, bool displayMessage = true);
    }
}