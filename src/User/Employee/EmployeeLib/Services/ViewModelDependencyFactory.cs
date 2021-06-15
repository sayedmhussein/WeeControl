using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using MySystem.User.Employee.Interfaces;

namespace MySystem.User.Employee.Services
{
    public class ViewModelDependencyFactory : IViewModelDependencyFactory
    {
        public HttpClient HttpClientInstance { get; private set; }

        public ILogger Logger { get; private set; }

        public IDevice Device { get; private set; }

        public string AppDataPath { get; private set; }


        public ViewModelDependencyFactory(HttpClient httpClient, IDevice device, string dataPath)
            :this(httpClient, device, dataPath, null)
        {
        }

        public ViewModelDependencyFactory(HttpClient httpClient, IDevice device, string dataPath, ILogger logger)
        {
            HttpClientInstance = httpClient ?? throw new ArgumentNullException("You must pass HttpClient to constructor.");
            Device = device ?? throw new ArgumentNullException("You must pass device to constructor.");
            AppDataPath = dataPath ?? throw new ArgumentNullException();
            Logger = logger;
        }
    }
}
