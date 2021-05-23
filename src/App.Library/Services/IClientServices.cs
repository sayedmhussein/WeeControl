using System;
using System.Net.Http;
using System.Threading.Tasks;
using MySystem.Web.ClientService.Configuration;
using MySystem.Web.Shared.Configuration.Models;
using Microsoft.Extensions.Logging;

namespace MySystem.Web.ClientService.Services
{
    public interface IClientServices
    {
        #region Constructor Injections
        IDevice Device { get; }
        ILogger Logger { get; }
        IApi Api { get; }
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