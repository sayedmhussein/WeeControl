using System.Net.Http;
using Microsoft.Extensions.Logging;
using MySystem.SharedKernel.Interfaces;
using MySystem.User.Employee.Configuration;

namespace MySystem.User.Employee.Services
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