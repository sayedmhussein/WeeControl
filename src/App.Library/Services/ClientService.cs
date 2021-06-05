using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MySystem.Persistence.ClientService.Configuration;
using MySystem.Persistence.Shared.Configuration.Models;

namespace MySystem.Persistence.ClientService.Services
{
    public class ClientServices : IClientServices
    {
        #region Private Properties
        private static HttpClient httpClientInstance;
        
        private readonly HttpMessageHandler handler;
        #endregion

        #region Public Properties
        public Config Settings { get; private set; }
        public IApi Api { get; private set; }
        public ILogger Logger { get; private set; }
        public IDevice Device { get; private set; }
        
        public HttpClient HttpClientInstance
        {
            get
            {
                if (SystemUnderTest)
                {
                    var c = handler == null ?  new HttpClient() : new HttpClient(handler);
                    c.BaseAddress = Api?.Base;
                    c.DefaultRequestHeaders.Add("Accept-version", Api?.Version);
                    return c;

                }
                else
                {
                    if (httpClientInstance == null)
                    {
                        httpClientInstance = handler == null ? new HttpClient() : new HttpClient(handler);
                        httpClientInstance.BaseAddress = Api?.Base;
                        httpClientInstance.DefaultRequestHeaders.Add("Accept-version", Api?.Version);
                    }
                    Logger.LogTrace("static HttClient was called.");
                    return httpClientInstance;
                }
            }
        }

        public string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public bool SystemUnderTest { get; set; } = false;
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
        public ClientServices(IDevice device, IApi api)
            : this(device, api, null)
        {
        }

        public ClientServices(IDevice device, IApi api, ILogger logger)
            : this(device, api, logger, null)
        {  
        }

        public ClientServices(IDevice device, IApi api, ILogger logger, HttpMessageHandler handler)
            : this(device, api, logger, handler, false)
        {
        }

        public ClientServices(IDevice device, IApi api, ILogger logger, HttpMessageHandler handler, bool systemUnderTest)
        {
            Device = device ?? throw new ArgumentNullException("You must pass device to constructor.");
            Api = api ?? throw new ArgumentNullException("You must pass api to constructor.");

            this.Logger = logger;
            this.handler = handler;

            Settings = Config.GetInstance();
            SystemUnderTest = systemUnderTest;
        }
        #endregion
    }
}
