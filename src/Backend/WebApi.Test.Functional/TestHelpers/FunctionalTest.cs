using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Routing;

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public sealed class FunctionalTest : IFunctionalTest
    {
        protected readonly CustomWebApplicationFactory<Startup> factory;

        public FunctionalTest(CustomWebApplicationFactory<Startup> factory, HttpMethod method, string deviceid)
        {
            this.factory = factory;

            RequestMessage = new HttpRequestMessage
            {
                Method = method,
                Version = new Version("1.0")
            };

            DeviceId = deviceid;
        }

        public void Dispose()
        {
            RequestMessage = null;
            Client = null;
        }

        private HttpClient client;
        public HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = factory.CreateClient();
                }
                return client;
            }
            set
            {
                client = value;
            }
        }

        public HttpRequestMessage RequestMessage { get; set; }

        public string DeviceId { get; set; }

        public async Task<HttpRequestMessage> CloneRequestMessageAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Method = request.Method,
                Version = request.Version,
                RequestUri = request.RequestUri
            };

            if (request.Content != null)
            {
                var ms = new MemoryStream();
                await request.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                // Copy the content headers
                if (request.Content.Headers != null)
                    foreach (var h in request.Content.Headers)
                        clone.Content.Headers.Add(h.Key, h.Value);
            }

            foreach (KeyValuePair<string, object> option in request.Options)
                clone.Options.Set(new HttpRequestOptionsKey<object>(option.Key), option.Value);


            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }

        public HttpContent GetHttpContentAsJson(ISerializable dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        public Task<HttpResponseMessage> GetResponseMessageAsync(Uri requestUri, HttpContent content = null, string token = null)
        {
            RequestMessage.RequestUri = requestUri;
            RequestMessage.Content = content;

            if (string.IsNullOrEmpty(token) == false)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return Client.SendAsync(RequestMessage);
        }

        public Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage requestMessage, string token = null)
        {
            if (string.IsNullOrEmpty(token) == false)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return Client.SendAsync(requestMessage);
        }

        public Uri GetUri(ApiRouteEnum route)
        {
            return new Uri(new Uri(new ApiRoute().GetRoute(ApiRouteEnum.Base)), new ApiRoute().GetRoute(route));
        }
    }
}
