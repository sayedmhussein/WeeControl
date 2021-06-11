using System.Net.Http;
using Microsoft.Extensions.Logging;
using MySystem.Persistence.ClientService.Configuration;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Persistence.ClientService.Services
{
    public interface IClientServices
    {
        #region Constructor Injections
        IDevice Device { get; }
        ILogger Logger { get; }
        ISharedValues SharedValues { get; }
        #endregion

        #region Debugging and Testing
        bool SystemUnderTest { get; set; }
        #endregion

        #region Self Created
        string AppDataPath { get; }
        Config Settings { get; }
        #endregion

        HttpClient HttpClientInstance { get; }
    }
}