using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sayed.MySystem.Shared.Configuration.Models;

namespace Sayed.MySystem.ClientService.Services
{
    public class ClientServices : IClientServices
    {
        #region Private Properties
        private static HttpClient httpClient;

        private readonly IDevice device;
        #endregion

        #region Public Properties
        public Setting Settings { get; private set; }
        public IApi Api { get; private set; }

        public HttpClient HttpClient
        {
            get
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", device?.Token);
                return httpClient;
            }
            set => httpClient = value;
        }

        public string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        #endregion

        #region Constructors
        public ClientServices(IDevice device, IApi api) : this(device, api, null)
        {  
        }

        public ClientServices(IDevice device, IApi api, HttpMessageHandler handler)
        {
            this.device = device;
            Api = api;
            ConstructSettingInstance();
            PrepareHttpClient(handler);
        }
        #endregion

        #region Public Functions
        public void LogAppend(string arg, string filename = "logger.log")
        {
            lock (nameof(filename))
            {
                var file = Path.Combine(AppDataPath, filename);
                if (File.Exists(file) == false)
                {
                    File.Create(file);
                }

                using StreamWriter sw = File.AppendText(file);
                sw.WriteLine(arg);
            }
        }

        public string LogReadAll(string filename = "logger.log")
        {
            lock (nameof(filename))
            {
                var file = Path.Combine(AppDataPath, filename);
                if (File.Exists(file))
                {
                    return File.ReadAllText(file);
                }
                else
                {
                    return "No Log File Found.";
                }
            }
        }

        public void LogDeleteAll(string filename = "logger.log")
        {
            lock (nameof(filename))
            {
                var file = Path.Combine(AppDataPath, filename);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
        #endregion

        #region Private Functions
        private void PrepareHttpClient(HttpMessageHandler handler)
        {
            if (httpClient == null)
            {
                httpClient = handler == null ? new HttpClient() : new HttpClient(handler);
            }

            HttpClient.BaseAddress = Api.Base;
            httpClient.DefaultRequestHeaders.Add("Accept-version", Api.Version);

        }

        private void ConstructSettingInstance()
        {
            var appsettingResouceStream = Assembly.GetAssembly(typeof(Setting)).GetManifestResourceStream("Sayed.MySystem.ClientService.Configuration.values.json");
            if (appsettingResouceStream == null)
                return;

            using var streamReader = new StreamReader(appsettingResouceStream);
            var jsonStream = streamReader.ReadToEnd();
            Settings = JsonConvert.DeserializeObject<Setting>(jsonStream);

        }
        #endregion
    }
}
