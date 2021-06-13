using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.User.Employee.Interfaces;
using Newtonsoft.Json;

namespace MySystem.User.Employee.Services
{
    public class ViewModelDependencyFactory : IViewModelDependencyFactory
    {
        #region Private Properties
        private static HttpClient httpClientInstance;
        //
        private readonly HttpMessageHandler handler;
        private readonly Uri apiBaseUri;
        private readonly string apiVersion;
        #endregion

        #region Public Properties
        //public Config Settings { get; private set; }
        public ISharedValues SharedValues { get; private set; }
        public ILogger Logger { get; private set; }
        public IDevice Device { get; private set; }
        
        public HttpClient HttpClientInstance
        {
            get
            {
                if (SystemUnderTest)
                {
                    var c = handler == null ?  new HttpClient() : new HttpClient(handler);
                    c.BaseAddress = apiBaseUri;
                    c.DefaultRequestHeaders.Add("Accept-version", apiVersion);
                    return c;

                }
                else
                {
                    if (httpClientInstance == null)
                    {
                        httpClientInstance = handler == null ? new HttpClient() : new HttpClient(handler);
                        httpClientInstance.BaseAddress = apiBaseUri;
                        httpClientInstance.DefaultRequestHeaders.Add("Accept-version", apiVersion);
                    }
                    Logger?.LogTrace("static HttClient was called.");
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
        public ViewModelDependencyFactory(IDevice device, ISharedValues sharedValues)
            : this(device, sharedValues, null)
        {
        }

        public ViewModelDependencyFactory(IDevice device, ISharedValues sharedValues, ILogger logger)
            : this(device, sharedValues, logger, (HttpMessageHandler)null)
        {  
        }

        public ViewModelDependencyFactory(IDevice device, ISharedValues sharedValues, ILogger logger, HttpClient httpClient)
            : this(device, sharedValues, logger)
        {
            httpClientInstance = httpClient;
        }

        public ViewModelDependencyFactory(IDevice device, ISharedValues sharedValues, ILogger logger, HttpMessageHandler handler)
            : this(device, sharedValues, logger, handler, false)
        {
        }

        public ViewModelDependencyFactory(IDevice device, ISharedValues sharedValues, ILogger logger, HttpMessageHandler handler, bool systemUnderTest)
        {
            Device = device ?? throw new ArgumentNullException("You must pass device to constructor.");
            SharedValues = sharedValues ?? throw new ArgumentNullException("You must pass Shared Values Object to constructor.");

            this.Logger = logger;
            this.handler = handler;

            apiBaseUri = new Uri(SharedValues.ApiRoute[ApiRouteEnum.Base]);
            apiVersion = SharedValues.ApiRoute[ApiRouteEnum.Version];

            //Settings = Config.GetInstance();
            SystemUnderTest = systemUnderTest;
        }
        #endregion
    }
}
