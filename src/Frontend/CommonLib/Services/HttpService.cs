using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.CommonLib.Interfaces;

namespace WeeControl.Frontend.CommonLib.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILocalStorage localStorage;

        public HttpService(IHttpClientFactory httpClientFactory, ILocalStorage localStorage)
        {
            this.httpClientFactory = httpClientFactory;
            this.localStorage = localStorage;
        }
        
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
        {
            httpRequestMessage.Version = new Version("1.0");
            var client = httpClientFactory.CreateClient(IHttpService.UnSecuredApi);

            var token = await localStorage.GetItem<string>("Token");
            if (string.IsNullOrWhiteSpace(token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return await client.SendAsync(httpRequestMessage);
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