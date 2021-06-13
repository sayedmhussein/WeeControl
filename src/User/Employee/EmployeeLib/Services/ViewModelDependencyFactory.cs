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
        public HttpClient HttpClientInstance { get; private set; }

        public ISharedValues SharedValues { get; private set; }

        public ILogger Logger { get; private set; }

        public IDevice Device { get; private set; }

        public string AppDataPath { get; private set; }
        //public HttpClient HttpClientInstance
        //{
        //    get
        //    {
        //        if (SystemUnderTest)
        //        {
        //            var c = handler == null ?  new HttpClient() : new HttpClient(handler);
        //            c.BaseAddress = apiBaseUri;
        //            c.DefaultRequestHeaders.Add("Accept-version", apiVersion);
        //            return c;

        //        }
        //        else
        //        {
        //            if (httpClientInstance == null)
        //            {
        //                httpClientInstance = handler == null ? new HttpClient() : new HttpClient(handler);
        //                httpClientInstance.BaseAddress = apiBaseUri;
        //                httpClientInstance.DefaultRequestHeaders.Add("Accept-version", apiVersion);
        //            }
        //            Logger?.LogTrace("static HttClient was called.");
        //            return httpClientInstance;
        //        }
        //    }
        //}

        //public string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);


        //#region Public Functions
        //public Task<HttpResponseMessage> GetResponseAsync(HttpMethod method, Uri uri)
        //{
        //    return GetResponseAsync<object>(method, uri, null);
        //}

        //public Task<HttpResponseMessage> GetResponseAsync<TRequest>(HttpMethod method, Uri uri, TRequest request)
        //{
        //    var requestMessage = new HttpRequestMessage(method, uri);
        //    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Device.Token);
        //    if (request != null)
        //    {
        //        var jsonObject = JsonConvert.SerializeObject(request);
        //        var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
        //        requestMessage.Content = content;
        //    }

        //    var response = HttpClientInstance.SendAsync(requestMessage);
        //    return response;
        //}
        //#endregion

        public ViewModelDependencyFactory(HttpClient httpClient, IDevice device, string dataPath, ISharedValues sharedKernel)
            :this(httpClient, device, dataPath, sharedKernel, null)
        {
        }

        public ViewModelDependencyFactory(HttpClient httpClient, IDevice device, string dataPath, ISharedValues sharedKernel, ILogger logger)
        {
            HttpClientInstance = httpClient ?? throw new ArgumentNullException("You must pass HttpClient to constructor.");
            Device = device ?? throw new ArgumentNullException("You must pass device to constructor.");
            AppDataPath = dataPath ?? throw new ArgumentNullException();
            SharedValues = sharedKernel ?? throw new ArgumentNullException("You must pass Shared Values Object to constructor.");
            Logger = logger;
        }
    }
}
