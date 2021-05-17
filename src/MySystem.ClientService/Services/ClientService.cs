using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sayed.MySystem.ClientService.Configuration;
using Sayed.MySystem.Shared.Configuration.Models;

namespace Sayed.MySystem.ClientService.Services
{
    public class ClientServices : IClientServices
    {
        #region Private Properties
        private static HttpClient httpClientInstance;
        private readonly bool systemUnderTest;
        private readonly HttpMessageHandler handler;
        #endregion

        #region Public Properties
        public Config Settings { get; private set; }
        public IApi Api { get; private set; }
        public IDevice Device { get; private set; }
        public ILogger Logger { get; private set; }

        public HttpClient HttpClientInstance
        {
            get
            {
                if (systemUnderTest)
                {
                    var c = handler == null ?  new HttpClient() : new HttpClient(handler);
                    c.BaseAddress = Api?.Base;
                    c.DefaultRequestHeaders.Add("Accept-version", Api?.Version);
                    return c;

                }
                else
                {
                    return httpClientInstance;
                }
            }
        }

        public string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        #endregion

        #region Public Functions
        public Task<HttpResponseMessage> GetResponseAsync(HttpMethod method, Uri uri)
        {
            return GetResponseAsync<object>(method, uri, null);
        }

        public Task<HttpResponseMessage> GetResponseAsync<TRequest>(HttpMethod method, Uri uri, TRequest request)
        {
            var requestMessage = new HttpRequestMessage(method, uri);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Device.Token);
            if (request != null)
            {
                var jsonObject = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                requestMessage.Content = content;
            }

            var response = HttpClientInstance.SendAsync(requestMessage);
            return response;
        }
        #endregion

        #region Constructors
        public ClientServices(IDevice device, IApi api, ILogger logger, bool systemUnderTest = false)
            : this(device, api, logger, null, systemUnderTest)
        {  
        }

        public ClientServices(IDevice device, IApi api, ILogger logger, HttpMessageHandler handler, bool systemUnderTest = false)
        {
            Device = device ?? throw new ArgumentNullException("You must pass device to constructor.");
            Api = api ?? throw new ArgumentNullException("You must pass api to constructor.");
            Logger = logger ?? throw new ArgumentNullException("You must pass logger to constructor.");

            this.systemUnderTest = systemUnderTest;
            this.handler = handler;

            Settings = Config.GetInstance();

            if (httpClientInstance == null)
            {
                httpClientInstance = handler == null ? new HttpClient() : new HttpClient(handler);
                httpClientInstance.BaseAddress = Api?.Base;
                httpClientInstance.DefaultRequestHeaders.Add("Accept-version", Api?.Version);
            }
        }
        #endregion
    }
}
