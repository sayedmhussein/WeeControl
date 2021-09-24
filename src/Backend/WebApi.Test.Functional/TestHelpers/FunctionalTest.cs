using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Newtonsoft.Json;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Routing;

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public sealed class FunctionalTest : IFunctionalTest
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly string device;
        private readonly HttpRequestMessage defaultRequestMessage;
        private HttpClient client;

        public FunctionalTest(CustomWebApplicationFactory<Startup> factory, string device, HttpRequestMessage defaultRequestMessage)
        {
            this.factory = factory;
            this.device = device;
            this.defaultRequestMessage = defaultRequestMessage;
        }
        
        [Obsolete]
        public FunctionalTest(CustomWebApplicationFactory<Startup> factory, string deviceid, HttpMethod method, string version)
        {
            this.factory = factory;

            RequestMessage = new HttpRequestMessage
            {
                Method = method,
                Version = new Version(version)
            };

            DeviceId = deviceid;
        }

        public void Dispose()
        {
            RequestMessage = null;
            Client = null;
        }
        
        public async Task<HttpRequestMessage> CloneRequestMessageAsync(HttpRequestMessage request)
        {
            if (request is null)
            {
                request = defaultRequestMessage;
            }

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

        public Task<HttpResponseMessage> GetResponseMessageAsync()
        {
            return GetResponseMessageAsync(defaultRequestMessage);
        }

        public Task<HttpResponseMessage> GetResponseMessageAsync(string token)
        {
            return GetResponseMessageAsync(defaultRequestMessage, token);
        }

        public HttpRequestMessage RequestMessage { get; private set; }
        
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

        

        public string DeviceId { get; set; }

        

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
