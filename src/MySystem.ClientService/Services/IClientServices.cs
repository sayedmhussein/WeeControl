using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sayed.MySystem.ClientService.Configuration;
using Sayed.MySystem.Shared.Configuration.Models;
using Microsoft.Extensions.Logging;

namespace Sayed.MySystem.ClientService.Services
{
    public interface IClientServices
    {
        #region Constructor Injections
        IDevice Device { get; }
        IApi Api { get; }
        ILogger Logger { get; }
        #endregion

        #region Self Created
        string AppDataPath { get; }
        Config Settings { get; }
        #endregion

        [Obsolete]
        HttpClient HttpClientInstance { get; }
    }
}