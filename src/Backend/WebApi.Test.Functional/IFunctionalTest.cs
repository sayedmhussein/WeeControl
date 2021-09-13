using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeeControl.SharedKernel.Helpers;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Obsolutes;

namespace WeeControl.Backend.WebApi.Test.Functional
{
    public interface IFunctionalTest : IDisposable
    {
        HttpClient Client { get; set; }
        HttpRequestMessage RequestMessage { get; set; }
        string DeviceId { get; set; }


        Uri GetUri(ApiRouteEnum route);
        HttpContent GetHttpContentAsJson(ISerializable dto);

        Task<HttpRequestMessage> CloneRequestMessageAsync(HttpRequestMessage requestMessage);

        Task<HttpResponseMessage> GetResponseMessageAsync(Uri requestUri, HttpContent content = null, string token = null);
        Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage requestMessage, string token = null);
    }
}
