using System;

namespace MySystem.ClientService.Interfaces
{
    public interface IAppSettings
    {
        string ApiBase { get; set; }
        string ApiVersion { get; set; }

        string LoginDisclaimer { get; set; }
        string HomeDisclaimer { get; set; }

    }
}
