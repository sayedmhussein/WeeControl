using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Backend.WebApi.Test.Controllers
{
    public interface IHttpMessage
    {
        string DeviceId { get; }

        Uri GetUri(ApiRouteEnum route);
        HttpContent GetHttpContentAsJson(ISerializable dto);
        Task<HttpRequestMessage> CloneAsync(HttpRequestMessage requestMessage);

        Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage requestMessage, string token = null);
    }
}
