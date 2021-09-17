using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.CommonLib.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            httpRequestMessage.Version = new Version("1.0");
            var client = httpClientFactory.CreateClient("NoAuth");

            return client.SendAsync(httpRequestMessage);
        }

        public Task<HttpResponseMessage> SendAsync(HttpMethod method, string relativeRoute, HttpContent content)
        {
            HttpRequestMessage requestMessage = new()
            {
                Method = method,
                RequestUri = new Uri(relativeRoute, UriKind.Relative),
                Content = content
            };

            return SendAsync(requestMessage);
        }

        public HttpContent GetHttpContentAsJson(ISerializable dto)
        {
            var content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}