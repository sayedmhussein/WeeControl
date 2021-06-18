using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IViewModelDependencyFactory
    {
        HttpClient HttpClientInstance { get; }

        IDevice Device { get; }

        ILogger Logger { get; }

        string AppDataPath { get; }
    }
}