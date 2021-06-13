using System.Net.Http;
using Microsoft.Extensions.Logging;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.User.Employee.Interfaces
{
    public interface IViewModelDependencyFactory
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
        //Config Settings { get; }
        #endregion

        HttpClient HttpClientInstance { get; }
    }
}