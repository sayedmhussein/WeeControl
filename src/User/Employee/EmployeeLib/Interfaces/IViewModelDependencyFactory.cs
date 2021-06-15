using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace MySystem.User.Employee.Interfaces
{
    public interface IViewModelDependencyFactory
    {
        HttpClient HttpClientInstance { get; }

        IDevice Device { get; }

        ILogger Logger { get; }

        string AppDataPath { get; }
    }
}