using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public sealed class FunctionalTestService : IFunctionalTestService
    {
        public static HttpContent GetHttpContentAsJson(object dto)
        {
            return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        }
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private HttpClient client;

        public FunctionalTestService(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        public void Dispose()
        {
            Client = null;
        }

        public async Task<HttpRequestMessage> CloneRequestMessageAsync(HttpRequestMessage request)
        {
            if (request is null)
            {
                throw new ArgumentNullException("You didn't pass HttpRequestMessage.");
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

        
        

        

        public HttpContent GetHttpContentAsJson(ISerializable dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        public Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage requestMessage, string token = null)
        {
            if (string.IsNullOrEmpty(token) == false)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return Client.SendAsync(requestMessage);
        }
    }
}
