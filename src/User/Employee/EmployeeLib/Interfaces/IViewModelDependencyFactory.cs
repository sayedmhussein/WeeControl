using System.Net.Http;
using Microsoft.Extensions.Logging;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.User.Employee.Interfaces
{
    public interface IViewModelDependencyFactory
    {
        HttpClient HttpClientInstance { get; }

        IDevice Device { get; }

        ILogger Logger { get; }

        ISharedValues SharedValues { get; }

        string AppDataPath { get; }
    }
}