using System;

namespace MySystem.ClientService.Interfaces
{
    public interface IAppSettings
    {
        [Obsolete]
        string WelComeText { get; set; }

        string ApiBase { get; set; }
        string ApiVersion { get; set; }
    }
}
