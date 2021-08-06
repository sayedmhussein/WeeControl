using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Server.WebApi.Test.Controllers
{
    public class FunctionalBaseTest<T> where T : class
    {
        public static Uri GetUri(ApiRouteEnum route)
        {
            return new Uri(new Uri(new CommonLists().GetRoute(ApiRouteEnum.Base)), new CommonLists().GetRoute(route));
        }

        internal RequestDto<T> RequestDto { get; set; }

        protected readonly HttpClient client;
        private readonly string version;
        private readonly HttpMethod httpMethod;
        private readonly ApiRouteEnum apiRoute;

        private HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return new HttpRequestMessage()
                {
                    Method = httpMethod,
                    Version = new Version(version),
                    Content = GetHttpContentAsJson(RequestDto),
                    RequestUri = GetUri(apiRoute)
                };
            }
        }

        internal FunctionalBaseTest(HttpClient client, string version, HttpMethod httpMethod, ApiRouteEnum apiRoute)
        {
            this.client = client;
            this.version = version;
            this.httpMethod = httpMethod;
            this.apiRoute = apiRoute;
        }

        internal Task<HttpResponseMessage> GetResponseMessageAsync(string token)
        {
            if (string.IsNullOrEmpty(token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return client.SendAsync(HttpRequestMessage);
        }

        internal Task<HttpResponseMessage> GetResponseMessageAsync(string token, HttpRequestMessage requestMessage)
        {
            if (string.IsNullOrEmpty(token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return client.SendAsync(requestMessage);
        }

        protected HttpContent GetHttpContentAsJson(ISerializable dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}
