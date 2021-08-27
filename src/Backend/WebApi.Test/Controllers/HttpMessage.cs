using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Backend.WebApi.Test.Controllers
{
    public class HttpMessage : IHttpMessage 
    {
        private readonly HttpClient client;

        public string DeviceId { get; private set; }

        internal HttpMessage(HttpClient client, string deviceId)
        {
            this.client = client;
            DeviceId = deviceId;
        }

        public Uri GetUri(ApiRouteEnum route)
        {
            return new Uri(new Uri(new CommonLists().GetRoute(ApiRouteEnum.Base)), new CommonLists().GetRoute(route));
        }

        public HttpContent GetHttpContentAsJson(ISerializable dto)
        {
            string content = JsonConvert.SerializeObject(dto);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        public async Task<HttpRequestMessage> CloneAsync(HttpRequestMessage request)
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

        public Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage requestMessage, string token)
        {
            if (string.IsNullOrEmpty(token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return client.SendAsync(requestMessage);
        }
    }
}
