using System.Net.Http;
using System.Threading.Tasks;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.CommonLib.Interfaces
{
    public interface IHttpService
    {
        public const string UnSecuredApi = "NoAuth";
        public const string SecuredApi = "WebAPI";
        
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);

        Task<HttpResponseMessage> SendAsync(HttpMethod method, string relativeRoute, HttpContent content);

        HttpContent GetHttpContentAsJson(ISerializable serializableDto);
    }
}